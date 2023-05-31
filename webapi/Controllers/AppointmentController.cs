using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Przychodnia.Shared;
using Przychodnia.Webapi.Data;
using Przychodnia.Webapi.Models;
using System.Security.Claims;

namespace Przychodnia.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    /*[Authorize(Roles = "Patient")]*/
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;

        public AppointmentController(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var appointments = await _db.Appointments.Include(a => a.Doctor ).Include(a => a.Patient)
                .Select(a => new
                {
                    a.Id,
                    a.Date,
                    a.Specialization,
                    a.Symptoms,
                    a.Medicines,
                    a.Diagnosis,
                    a.Recommendations,
                    a.Finished,
                    Patient = a.Patient != null ? new
                    {
                        a.Patient.Id,
                        a.Patient.FirstName,
                        a.Patient.LastName,
                        a.Patient.DateOfBirth
                    } : null,
                    Doctor = a.Doctor != null ? new
                    {
                        a.Doctor.Id,
                        a.Doctor.FirstName,
                        a.Doctor.LastName,
                        a.Doctor.Specialization
                    } : null
                }).ToListAsync();
            return Ok(appointments);
        }

        [HttpGet("specialization/{specialization}/year/{year}/month/{month}")]
        public async Task<IActionResult> GetAvailableDaysInMonth([FromRoute] int year, [FromRoute] int month, [FromRoute] int specialization)
        {
            var slots_week = 16;
            var slots_saturday = 10;
            var employees_count = _db.Employees.Select(x => x.Specialization == (Specialization) specialization).ToList().Count;
            var appointments_month = (await _db.Appointments
                .Where(a => a.Date.Year == year && a.Date.Month == month && a.Specialization == (Specialization) specialization)
                .Select(a => new { a.Specialization, a.Date }).GroupBy(a => a.Date)
                .Select(g => new { key = DateOnly.FromDateTime(g.Key), available = 
                    g.Key.DayOfWeek == DayOfWeek.Saturday ? g.Count() < slots_saturday*employees_count : 
                    g.Key.DayOfWeek == DayOfWeek.Sunday ? false :
                    g.Count() < slots_week*employees_count}).ToListAsync())
                    .Where(a => a.available == false).Select(a => a.key);
            return Ok(appointments_month);
        }

        [HttpGet("available-hours/{receivedDate}/specialization/{specialization}")]
        public async Task<IActionResult> GetAvailableHours([FromRoute] string receivedDate, [FromRoute] int specialization)
        {
            DateTime date = DateTime.Parse(receivedDate);
            //if (date == null || specialization == null) return BadRequest("Object is null");
            var employees_count = _db.Employees.Select(x => x.Specialization == (Specialization)specialization).ToList().Count;
            var hours = (await _db.Appointments
                .Where(a => a.Date.Date.CompareTo(date) == 0 && a.Specialization == (Specialization)specialization)
                .Select(a => new { a.Date.TimeOfDay }).GroupBy(a => a.TimeOfDay)
                .Select(g => new { key = g.Key, available = g.Count() < employees_count })
                .Where(g => g.available == false).ToListAsync()).Select(h => h.key);
            return Ok(hours);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Appointment), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById(int id)
        {
            var appointment = await _db.Appointments.Select(a => new
            {
                a.Id,
                a.Date,
                a.Finished,
                a.Specialization,
                Doctor = a.Doctor != null ? new
                {
                    a.Doctor.FirstName,
                    a.Doctor.LastName,
                    a.Doctor.Specialization
                } : null,
                Patient = a.Patient != null ? new
                {
                    a.Patient.Id,
                    a.Patient.FirstName,
                    a.Patient.LastName,
                } : null,
                a.Medicines,
                a.Symptoms,
                a.Diagnosis,
                a.Recommendations,
                a.ControlAppointment
            }).FirstOrDefaultAsync(a => a.Id == id);
            return appointment == null ? NotFound() : Ok(appointment);
        }
        [Authorize(Roles = "Patient")]
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateAppointmentDto dto)
        {
            // Poniedziałek-piątek 9:00-17:00, Sobota 9:00-14:00
            if (dto.Date.Minute % 30 != 0 ||
                (dto.Date.Hour >= 17 || dto.Date.Hour < 9
                || dto.Date.DayOfWeek.Equals(0) || (dto.Date.Hour >= 14
                || dto.Date.Hour < 9)
                && dto.Date.DayOfWeek.Equals(6))) return StatusCode(406, "Wrong time");
            if (dto.Date.CompareTo(DateTime.Now.AddHours(2)) < 0) return StatusCode(406, "Past date");
            DateTime date = new DateTime(dto.Date.Year, dto.Date.Month, dto.Date.Day, dto.Date.Hour, dto.Date.Minute, 0, dto.Date.Kind);
            int employees_count = _db.Employees.Count(emp => emp.Specialization == dto.Specialization);
            int appointments_count = _db.Appointments.Count(a => a.Date == date && a.Specialization == dto.Specialization);
            bool available = appointments_count < employees_count;
            if (!available) return StatusCode(406, "Date not available");
            string id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            if (_db.Appointments.Any(a => a.Date.CompareTo(date) == 0 && a.PatientId == id))
                return Conflict("You have already scheduled an appointment for this time.");
            var appointment = new Appointment();
            appointment.Date = date;
            appointment.Specialization = dto.Specialization;
            appointment.Symptoms = dto.Symptoms;
            appointment.Medicines = dto.Medicines;
            appointment.PatientId = id;

            await _db.Appointments.AddAsync(appointment);
            await _db.SaveChangesAsync();
            return StatusCode(201, new { message =  "Appointment created", data = appointment });
        }

        [HttpDelete("cancel-appointment/{id}")]
        public async Task<IActionResult> CancelAppointment([FromRoute] int id)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            var appointment = await _db.Appointments.FirstOrDefaultAsync(a => a.Id == id && a.PatientId == userId);
            if (appointment != null && (appointment.Date - DateTime.Now).TotalHours >= 24)
            {
                await _db.Appointments.Where(a => a.Id == id && a.PatientId == userId).ExecuteDeleteAsync();
                await _db.SaveChangesAsync();
                return Ok(new { message = "Appointment cancelled", data = new { id = id } });
            } else if (appointment != null && (appointment.Date - DateTime.Now).TotalHours < 24) 
                return Conflict("The visit can be canceled at least 24 hours before the date");
            return NotFound("Appointment not found");
        }

        [HttpGet("patient")]
        public IActionResult GetAppointmentsOfLoggedInUser()
        {
            string? id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id == null) return NotFound("User not found");

            var appointments = _db.Patients.Where(p => p.Id == id).Include(p => p.Appointments)
                .Select(p => p.Appointments.Select(a => new {
                    a.Date,
                    a.Id,
                    a.Finished,
                    a.Specialization,
                    Doctor = a.Doctor != null ? new
                    {
                        a.Doctor.FirstName,
                        a.Doctor.LastName,
                        a.Doctor.Specialization
                    } : null,
                    a.Medicines,
                    a.Recommendations,
                    a.Symptoms,
                    a.ControlAppointment
                })).ToList();
            return Ok(new { appointments = appointments });


        }
        [Authorize(Roles = "Employee")]
        [HttpGet("schedule")]
        public async Task<IActionResult> GetDoctorSchedule([FromBody] string? date)
        {
            string? id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (id == null) return NotFound("User not found");
            if (date != null)
            {
                DateTime newDate = DateTime.Parse(date);
                var appointments = await _db.Appointments.Where(a => a.DoctorId == id &&
                    a.Date.CompareTo(newDate) == 0).Select(a => new
                    {
                        Patient = a.Patient != null ? a.Patient.FirstName + " " + a.Patient.LastName : null,
                        a.Date,
                        a.Id,
                        a.Finished,
                        a.Medicines,
                        a.Recommendations,
                        a.Symptoms,
                        a.ControlAppointment
                    }).ToListAsync(); // chyba działa
                return Ok(appointments);
            }
            else
            {
                var appointments = await _db.Appointments.Where(a => a.DoctorId == id).Select(a => new
                   {
                       Patient = a.Patient != null ? a.Patient.FirstName + " " + a.Patient.LastName : null,
                       a.Date,
                       a.Id,
                       a.Finished,
                       a.Medicines,
                       a.Recommendations,
                       a.Symptoms,
                       a.ControlAppointment
                   }).ToListAsync(); // chyba działa
                return Ok(appointments);
            }
        }

        [Authorize(Roles = "Staff")]
        [HttpGet("everySchedule")]
        public async Task<IActionResult> GetAllDoctorsSchedule([FromBody] string? date)
        {
            if (date != null)
            {
                DateTime newDate = DateTime.Parse(date);
                var appointments = await _db.Appointments.Where(a=> a.Date.CompareTo(newDate) == 0).Select(a => new
                    {
                        Patient = a.Patient != null ? a.Patient.FirstName + " " + a.Patient.LastName : null,
                        a.Date,
                        a.Id,
                        a.Finished,
                        a.Medicines,
                        a.Recommendations,
                        a.Symptoms,
                        a.ControlAppointment
                    }).ToListAsync(); 
                return Ok(appointments);
            }
            else
            {
                var appointments = await _db.Appointments.Select(a => new
                   {
                       Patient = a.Patient != null ? a.Patient.FirstName + " " + a.Patient.LastName : null,
                       a.Date,
                       a.Id,
                       a.Finished,
                       a.Medicines,
                       a.Recommendations,
                       a.Symptoms,
                       a.ControlAppointment
                   }).ToListAsync(); 
                return Ok(appointments);
            }
        }

        [HttpPatch("update/{id}")]

        public async Task<IActionResult> UpdateAppointment([FromRoute] int id, [FromBody] UpdateAppointmentDto dto)
        {
            if (dto == null) return BadRequest("Object is null");

            var appointment = await _db.Appointments.FindAsync(id);
            if (appointment == null) return BadRequest("Appointment not found");

            foreach (var prop in typeof(Appointment).GetProperties())
            {
                var fromProp = typeof(UpdateAppointmentDto).GetProperty(prop.Name);
                var toValue = fromProp != null ? fromProp.GetValue(dto, null) : null;
                if (toValue != null)
                {
                    prop.SetValue(appointment, toValue, null);
                    _db.Entry(appointment).Property(prop.Name).IsModified = true;
                }
            }
            await _db.SaveChangesAsync();
            return Ok("Updated");
        }

        [Authorize(Roles = "Staff")]
        [HttpGet("awaiting-appointments")]
        public async Task<IActionResult> GetAwaitingAppointments()
        {
            var appointments = await _db.Appointments.Where(a => a.DoctorId == null).OrderBy(a => a.Date).ToListAsync();
            return Ok(appointments);
        }

        [HttpPatch("assign-appointment/{appointmentId}")]
        public async Task<IActionResult> AssignAppointment([FromRoute] string appointmentId, [FromBody] string doctorId)
        {
            if (doctorId == null) return BadRequest("Doctor id is null");

            string? id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var appointment = await _db.Appointments.FirstOrDefaultAsync(a => a.Id == Int32.Parse(appointmentId));
            if (appointment == null) return NotFound("Appointment not found");
            appointment.DoctorId = doctorId;
            _db.Entry(appointment).Property(p => p.DoctorId).IsModified = true;
            await _db.SaveChangesAsync();
            return Ok(new { message = "Appointment assigned", data = appointment });
        }

        [HttpPatch("finish/{id}")]
        public async Task<IActionResult> FinishAppointment([FromRoute] int id, [FromBody] FinishAppointmentDto dto)
        {
            Console.Write(dto.Diagnosis);
            if (dto == null) return BadRequest("Object is null");
            string? doctorId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var appointment = await _db.Appointments.FindAsync(id);
            if (appointment == null) return BadRequest("Appointment not found");
            if (appointment.DoctorId != doctorId) return Unauthorized("Cannot finish this appointment");
            foreach (var prop in typeof(Appointment).GetProperties())
            {
                var fromProp = typeof(FinishAppointmentDto).GetProperty(prop.Name);
                var toValue = fromProp != null ? fromProp.GetValue(dto, null) : null;
                if (toValue != null)
                {
                    prop.SetValue(appointment, toValue, null);
                    _db.Entry(appointment).Property(prop.Name).IsModified = true;
                }
            }
            await _db.SaveChangesAsync();
            return Ok("Appointment finished");
        }
    }
}
