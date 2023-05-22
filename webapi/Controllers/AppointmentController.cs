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
        
        public AppointmentController(ApplicationDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<IEnumerable<Appointment>>Get() => await _db.Appointments.ToListAsync();

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Appointment), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById(int id){
            var appointment= await _db.Appointments.FindAsync(id);
            return appointment == null ? NotFound():Ok(appointment);
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody]Appointment obj)
        {
            await _db.Appointments.AddAsync(obj);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new {id = obj.Id}, obj);
        }

        [HttpGet("patient")]
        public IActionResult GetAppointmentsOfLoggedInUser()
        {
            string? id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id == null) return NotFound("User not found");

            var appointments = _db.Patients.Where(p => p.Id == id).Include(p => p.Appointments)
                .Select(p => p.Appointments.Select(a => new { a.Date, a. Id, a.Finished, 
                    a.Doctor, a.Medicines, a.Recommendations, a.Symptoms, a.ControlAppointment })).ToList();
            return Ok(new { appointments = appointments });

            
        }

        [HttpGet("schedule")]
        public async Task<IActionResult> GetDoctorSchedule()
        {
            string? id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id == null) return NotFound("Doctor not found");

            var appointments = await _db.Appointments.Where(a => a.DoctorId == id && 
                a.Date.Date.CompareTo(DateTime.Now.Date) >= 0)
                .Include(a => a.Patient).Select(a => new {
                    a.Date,
                    a.Id,
                    a.Finished,
                    a.Medicines,
                    a.Recommendations,
                    a.Symptoms,
                    a.ControlAppointment
                }).ToListAsync(); // nie działa

            return Ok(appointments);
        }
        /*  [HttpPatch("{id}")]
          [ProducesResponseType(typeof(Appointment), StatusCodes.Status402PaymentRequired)]
          [ProducesResponseType(StatusCodes.Status400BadRequest)]
          public async Task<IActionResult> Patch(int id, [FromBody] Appointment obj)
          {
              return Ok(200);
          }*/

        [HttpPatch("update/{id}")]

        public async Task<IActionResult> UpdateAppointment([FromRoute] int id ,[FromBody] UpdateAppointmentDto dto)
        {
            if (dto == null) return BadRequest("Object is null");

            var appointment = await _db.Appointments.FindAsync(id);
            if (appointment == null) return BadRequest("Cannot find appointment");

            foreach(var prop in typeof(Appointment).GetProperties())
            {
                var fromProp = typeof(UpdateAppointmentDto).GetProperty(prop.Name);
                var toValue = fromProp != null ? fromProp.GetValue(dto, null) : null;
                if (toValue != null)
                {
                    prop.SetValue(appointment, toValue, null);
                }
            }
            _db.Appointments.Update(appointment);
           /* _db.Entry(appointment).State = EntityState.Modified;*/ // nie wiem czy to coś daje
            await _db.SaveChangesAsync();
            return Ok(_db.Entry(appointment).State);
        }
    }
}
