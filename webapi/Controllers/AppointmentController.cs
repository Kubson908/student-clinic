using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using NuGet.Protocol;
using Przychodnia.Shared;
using Przychodnia.Webapi.Data;
using Przychodnia.Webapi.Models;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Claims;

namespace Przychodnia.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<Patient> _patientManager;

        public AppointmentController(ApplicationDbContext db, UserManager<Patient> patientManager)
        {
            _db = db;
            _patientManager = patientManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var appointments = await _db.Appointments.Include(a => a.Doctor).Include(a => a.Patient)
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
                    } : null,
                    a.AppointmentId,
                    ControlAppointment = a.AppointmentId != null ? new
                    {
                        a.ControlAppointment!.Date,
                        a.ControlAppointment.Id,
                        a.ControlAppointment.Finished,
                        a.ControlAppointment.Medicines,
                        a.ControlAppointment.Recommendations,
                        a.ControlAppointment.Symptoms,
                        a.PatientId,
                        a.ControlAppointment.DoctorId,
                    } : null
                }).ToListAsync();
            return Ok(appointments);
        }

        [HttpGet("specialization/{specialization}/year/{year}/month/{month}")]
        public async Task<IActionResult> GetAvailableDaysInMonth([FromRoute] int year, [FromRoute] int month, [FromRoute] int specialization)
        {
            var slots_week = 16;
            var slots_saturday = 10;
            var employees_count = _db.Employees.Where(x => x.Specialization == (Specialization)specialization).Select(x => x.Specialization).ToList().Count;
            var appointments_month = (await _db.Appointments
                .Where(a => a.Date.Year == year && a.Date.Month == month && a.Specialization == (Specialization)specialization)
                .Select(a => new { a.Specialization, a.Date }).GroupBy(a => a.Date)
                .Select(g => new { key = DateOnly.FromDateTime(g.Key), available =
                    g.Key.DayOfWeek == DayOfWeek.Saturday ? g.Count() < slots_saturday * employees_count :
                    g.Key.DayOfWeek == DayOfWeek.Sunday ? false :
                    g.Count() < slots_week * employees_count }).ToListAsync())
                    .Where(a => a.available == false).Select(a => a.key);
            return Ok(appointments_month);
        }

        [Authorize(Roles = "Patient, Employee")]
        [HttpGet("available-hours/{receivedDate}/specialization/{specialization}/{patientId}")]
        [HttpGet("available-hours/{receivedDate}/specialization/{specialization}")]
        public async Task<IActionResult> GetAvailableHours([FromRoute] string receivedDate, [FromRoute] int specialization, [FromRoute] string? patientId)
        {
            DateTime date = DateTime.Parse(receivedDate);
            //if (date == null || specialization == null) return BadRequest("Object is null");
            string id;
            if (HttpContext.User.FindFirstValue(ClaimTypes.Role) == "Employee" && patientId != null)
                id = patientId;
            else id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var employees_count = _db.Employees.Where(x => x.Specialization == (Specialization)specialization).Select(x => x.Specialization).ToList().Count;
            var hours = (await _db.Appointments
                .Where(a => a.Date.Date.CompareTo(date) == 0 && a.Specialization == (Specialization)specialization)
                .Select(a => new { a.Date.TimeOfDay }).GroupBy(a => a.TimeOfDay)
                .Select(g => new { key = g.Key, available = g.Count() < employees_count })
                .Where(g => g.available == false).ToListAsync()).Select(h => h.key);
            var patientHours = (await _db.Appointments
                .Where(a => a.Date.Date.CompareTo(date) == 0 && a.PatientId == id)
                .Select(a => new { a.Date.TimeOfDay }).GroupBy(a => a.TimeOfDay)
                .Select(g => new { key = g.Key, available = g.Count() < 1 })
                .Where(g => g.available == false).ToListAsync()).Select(h => h.key);
            var toReturn = hours.Union(patientHours).ToList();
            return Ok(toReturn);
        }

        [HttpGet("doctor/year/{year}/month/{month}")]
        public async Task<IActionResult> GetAvailableDaysInMonthByDoctor([FromRoute] int year, [FromRoute] int month)
        {
            var doctorId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var slots_week = 16;
            var slots_saturday = 10;
            var appointments_month = (await _db.Appointments
                .Where(a => a.Date.Year == year && a.Date.Month == month && a.DoctorId == doctorId)
                .Select(a => new { a.Specialization, a.Date }).GroupBy(a => a.Date)
                .Select(g => new { key = DateOnly.FromDateTime(g.Key), available =
                    g.Key.DayOfWeek == DayOfWeek.Saturday ? g.Count() < slots_saturday :
                    g.Key.DayOfWeek == DayOfWeek.Sunday ? false :
                    g.Count() < slots_week }).ToListAsync())
                    .Where(a => a.available == false).Select(a => a.key);
            return Ok(appointments_month);
        }

        [HttpGet("doctor/available-hours/{receivedDate}")]
        public async Task<IActionResult> GetAvailableHoursBydoctor([FromRoute] string receivedDate)
        {
            DateTime date = DateTime.Parse(receivedDate);
            var doctorId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var hours = (await _db.Appointments
                .Where(a => a.Date.Date.CompareTo(date) == 0 && a.DoctorId == doctorId)
                .Select(a => new { a.Date.TimeOfDay }).GroupBy(a => a.TimeOfDay)
                .Select(g => new { key = g.Key, available = g.Count() < 1 })
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
                a.AppointmentId,
                ControlAppointment = a.AppointmentId != null ? new
                {
                    a.ControlAppointment!.Date,
                    a.ControlAppointment.Id,
                    a.ControlAppointment.Finished,
                    a.ControlAppointment.Medicines,
                    a.ControlAppointment.Recommendations,
                    a.ControlAppointment.Symptoms,
                    a.PatientId,
                    a.ControlAppointment.DoctorId,
                } : null
            }).FirstOrDefaultAsync(a => a.Id == id);
            return appointment == null ? NotFound() : Ok(appointment);
        }
        [Authorize(Roles = "Patient")]
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateAppointmentDto dto)
        {
            var patient = await _db.Patients.Select(p => new
            {
                p.Id,
                p.Verified,
                Appointments = new
                {
                    List = p.Appointments.Select(a => a.Id).ToList()
                }
            }).FirstOrDefaultAsync(p => p.Id == HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (patient != null && !patient.Verified && patient.Appointments.List.Count() >= 1)
                return Forbid();
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
            return StatusCode(201, new { message = "Appointment created", data = appointment });
        }

        [HttpDelete("cancel-appointment/{id}")]
        public async Task<IActionResult> CancelAppointment([FromRoute] int id)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            var appointment = await _db.Appointments.FirstOrDefaultAsync(a => a.Id == id && a.PatientId == userId && !a.Finished);
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
                .Select(p => p.Appointments.Where(a => !a.Finished).Select(a => new {
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
        [Authorize(Roles = "Employee, Staff")]
        [HttpGet("schedule/{year}/{month}/{day}")]
        [HttpGet("schedule")]
        [HttpGet("doctor/{doctorId}/schedule/{year}/{month}/{day}")]
        [HttpGet("doctor/{doctorId}/schedule")]
        public async Task<IActionResult> GetDoctorSchedule([FromRoute] int? year, [FromRoute] int? month, [FromRoute] int? day, [FromRoute] string? doctorId)
        {
            var claims = ((ClaimsIdentity)User.Identity!).Claims;
            var role = claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value.ToString()).ToList();
            string id;
            if (doctorId != null && role.Contains("Staff")) id = doctorId;
            else if (!role.Contains("Staff")) id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            else return BadRequest("Error");
            if (year != null && month != null && day != null)
            {
                DateTime newDate = DateTime.Parse(year + "-" + month + "-" + day);
                var appointments = await _db.Appointments.Where(a => a.DoctorId == id &&
                    a.Date.Date.CompareTo(newDate) == 0).Select(a => new
                    {
                        Patient = a.Patient != null ? a.Patient.FirstName + " " + a.Patient.LastName : null,
                        a.Date,
                        a.Id,
                        a.Finished,
                        a.Medicines,
                        a.Recommendations,
                        a.Symptoms,
                        ControlAppointment = a.AppointmentId != null ? new
                        {
                            a.ControlAppointment!.Date,
                            a.ControlAppointment.Id,
                            a.ControlAppointment.Finished,
                            a.ControlAppointment.Medicines,
                            a.ControlAppointment.Recommendations,
                            a.ControlAppointment.Symptoms,
                            a.PatientId,
                            a.ControlAppointment.DoctorId,
                        } : null
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
                    ControlAppointment = a.AppointmentId != null ? new
                    {
                        a.ControlAppointment!.Date,
                        a.ControlAppointment.Id,
                        a.ControlAppointment.Finished,
                        a.ControlAppointment.Medicines,
                        a.ControlAppointment.Recommendations,
                        a.ControlAppointment.Symptoms,
                        a.PatientId,
                        a.ControlAppointment.DoctorId,
                    } : null
                }).ToListAsync(); // chyba działa
                return Ok(appointments);
            }
        }

        [Authorize(Roles = "Staff")]
        [HttpGet("everySchedule")]
        [HttpGet("everySchedule/{year}/{month}/{day}")]
        public async Task<IActionResult> GetAllDoctorsSchedule([FromRoute] int? year, [FromRoute] int? month, [FromRoute] int? day)
        {
            if (year != null && month != null && day != null)
            {
                DateTime newDate = DateTime.Parse(year + "-" + month + "-" + day);
                var appointments = await _db.Appointments.Where(a => a.Date.Date.CompareTo(newDate) == 0).Select(a => new
                {
                    Patient = a.Patient != null ? a.Patient.FirstName + " " + a.Patient.LastName : null,
                    a.Date,
                    a.Id,
                    a.Finished,
                    a.Medicines,
                    a.Recommendations,
                    a.Symptoms,
                    a.PatientId,
                    a.DoctorId,
                    ControlAppointment = a.AppointmentId != null ? new
                    {
                        a.ControlAppointment!.Date,
                        a.ControlAppointment.Id,
                        a.ControlAppointment.Finished,
                        a.ControlAppointment.Medicines,
                        a.ControlAppointment.Recommendations,
                        a.ControlAppointment.Symptoms,
                        a.PatientId,
                        a.ControlAppointment.DoctorId,
                    } : null
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
                    a.PatientId,
                    a.DoctorId,
                    ControlAppointment = a.AppointmentId != null ? new
                    {
                        a.ControlAppointment!.Date,
                        a.ControlAppointment.Id,
                        a.ControlAppointment.Finished,
                        a.ControlAppointment.Medicines,
                        a.ControlAppointment.Recommendations,
                        a.ControlAppointment.Symptoms,
                        a.PatientId,
                        a.ControlAppointment.DoctorId,
                    } : null
                }).ToListAsync();
                return Ok(appointments);
            }
        }

        [Authorize(Roles = "Staff")]
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
        public async Task<IActionResult> AssignAppointment([FromRoute] string appointmentId, [FromBody] DoctorIdDto doctorId)
        {
            if (doctorId == null) return BadRequest("Doctor id is null");
            var doctor = await _db.Employees.FindAsync(doctorId.DoctorId);
            if (doctor == null) return NotFound("Doctor not found");
            var appointment = await _db.Appointments.Include(a => a.Patient).FirstOrDefaultAsync(a => a.Id == Int32.Parse(appointmentId));
            if (appointment == null) return NotFound("Appointment not found");
            if (appointment.Specialization != doctor.Specialization) return Conflict("Cannot assign this appointment to this doctor");
            appointment.DoctorId = doctorId.DoctorId;
            if (doctorId.Date.HasValue) appointment.Date = (DateTime)doctorId.Date;
            _db.Entry(appointment).Property(p => p.DoctorId).IsModified = true;
            await _db.SaveChangesAsync();
            await SendAppointmentConfirmation(appointment.Patient!.Email!,
                DateOnly.FromDateTime(appointment.Date) + ", " + appointment.Date.TimeOfDay, 
                doctor.FirstName + " " + doctor.LastName, (Specialization)doctor.Specialization);
            return Ok(new { message = "Appointment assigned", data = appointment });
        }

        [HttpPatch("finish/{id}")]
        public async Task<IActionResult> FinishAppointment([FromRoute] int id, [FromBody] FinishAppointmentDto dto)
        {
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
            if (dto.ControlDate is not null)
            {
                var newDate = DateTime.Parse(dto.ControlDate.ToString() ?? "");
                if (newDate.Minute % 30 != 0 ||
                (newDate.Hour >= 17 || newDate.Hour < 9
                || newDate.DayOfWeek.Equals(0) || (newDate.Hour >= 14
                || newDate.Hour < 9)
                && newDate.DayOfWeek.Equals(6))) return StatusCode(406, "Wrong time");
                if (newDate.CompareTo(DateTime.Now.AddHours(2)) < 0) return StatusCode(406, "Past date");
                DateTime date = new DateTime(newDate.Year, newDate.Month, newDate.Day, newDate.Hour, newDate.Minute, 0, newDate.Kind);
                int employees_count = _db.Employees.Count(emp => emp.Specialization == appointment.Specialization);
                int appointments_count = _db.Appointments.Count(a => a.Date == date && a.Specialization == appointment.Specialization);
                bool available = appointments_count < employees_count;
                if (!available) return StatusCode(406, "Date not available");
                if (_db.Appointments.Any(a => a.Date.CompareTo(date) == 0 && a.PatientId == appointment.PatientId))
                    return Conflict("This patient has already scheduled an appointment for this time.");

                var controlAppointment = new Appointment()
                {
                    PatientId = appointment.PatientId,
                    DoctorId = appointment.DoctorId,
                    Date = date,
                    AppointmentId = appointment.Id,
                    Specialization = appointment.Specialization,
                    Medicines = appointment.Medicines,
                    Symptoms = appointment.Symptoms,
                };
                appointment.ControlAppointment = controlAppointment;

                await _db.Appointments.AddAsync(controlAppointment);
            }
            await _db.SaveChangesAsync();
            string? email = (await _patientManager.FindByIdAsync(appointment.PatientId))!.Email;
            await SendRecommendations(new RecommendationsMailDto()
            {
                Email = email!,
                Diagnosis = appointment.Diagnosis,
                Recommendations = appointment.Recommendations,
                Medicines = appointment.Medicines,
                ControlAppointmentDate = appointment.ControlAppointment != null ? (DateOnly.FromDateTime(appointment.ControlAppointment.Date) + ", " + appointment.ControlAppointment.Date.TimeOfDay) : string.Empty,
                AppointmentDate = DateOnly.FromDateTime(appointment.Date) + ", " + appointment.Date.TimeOfDay,
            });
            return Ok("Appointment finished");
        }

        [HttpGet("statistics/{year}/{month}")]
        public async Task<IActionResult> GetStatistics([FromRoute] int year, [FromRoute] int month)
        {
            var appointments = await _db.Appointments.Where(a => a.Date.Year == year && a.Date.Month == month).ToListAsync();
            if (appointments.Count() == 0) return NoContent();
            return Ok(appointments);
        }

        private static async Task SendRecommendations(RecommendationsMailDto dto)
        {
            string mailFrom = "przychodniaspaghettimafia@gmail.com";
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Przychodnia Studencka", mailFrom));
            message.To.Add(new MailboxAddress(dto.Email, dto.Email));
            message.Subject = "Podsumowanie wizyty w przychodni";
            var body = new BodyBuilder();
            body.HtmlBody = "<h3>Podsumowanie wizyty</h3>" +
                "<div style=\"color: black; display: inline\"><strong>Data wizyty:</strong></div><div style=\"width: 450px; color: black; display: inline\"> " + dto.AppointmentDate + "</div><br /><br />";
            if(dto.Diagnosis != string.Empty)
                body.HtmlBody += "<div style=\"color: black; display: inline\"><strong>Diagnoza:</strong></div><div style=\"color: black; display: inline\"><div style=\"width: 450px;\"> " + dto.Diagnosis + "</div></div><br /><br />";
            if(dto.Medicines != string.Empty)
                body.HtmlBody += "<div style=\"color: black; display: inline\"><strong>Leki:</strong></div><div style=\"color: black; display: inline\"><div style=\"width: 450px;\"> " + dto.Medicines + "</div></div><br /><br />";
            if(dto.Recommendations != string.Empty)
                body.HtmlBody += "<div style=\"color: black; display: inline\"><strong>Zalecenia:</strong></div><div style=\"color: black; display: inline\"><div style=\"width: 450px;\"> " + dto.Recommendations + "</div></div><br /><br />";
            if(dto.ControlAppointmentDate != string.Empty)
                body.HtmlBody += "<div style=\"color: black; display: inline\"><strong>Data wizyty kontrolnej:</strong></div><div style=\"color: black; display: inline\"> " + dto.ControlAppointmentDate + "</div><br /><br />";
            message.Body = body.ToMessageBody();
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, false);
                client.Authenticate(mailFrom, "fegdvxpcrnsjwlvn");
                client.Send(message);
                client.Disconnect(true);
            }
            return;
        }

        private static async Task SendAppointmentConfirmation(string email, string date, string doctor, Specialization spec)
        {
            var specialization = new List<string>{ "Internista", "Pulmonolog", "Okulista", "Gastrolog" };
            string mailFrom = "przychodniaspaghettimafia@gmail.com";
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Przychodnia Studencka", mailFrom));
            message.To.Add(new MailboxAddress(email, email));
            message.Subject = "Potwierdzenie rezerwacji wizyty";
            var body = new BodyBuilder();
            body.HtmlBody = "<h3>Potwierdzenie rezerwacji wizyty</h3>" +
                "<div style=\"color: black;\"><strong>Data wizyty:</strong> " + date + "</div><br />";
            body.HtmlBody += "<div style=\"color: black;\"><strong>Specjalizacja:</strong> " + specialization[((int)spec)] + "</div><br />";
            body.HtmlBody += "<div style=\"color: black;\"><strong>Doktor:</strong> " + doctor + "</div><br />";
            message.Body = body.ToMessageBody();
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, false);
                client.Authenticate(mailFrom, "fegdvxpcrnsjwlvn");
                client.Send(message);
                client.Disconnect(true);
            }
            return;
        }
    }
}
