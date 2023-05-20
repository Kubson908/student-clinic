using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

            var appointments = _db.Appointments.Where(a => a.PatientId == id).ToList();
            return Ok(new { appointments = appointments });

            
        }

        /*  [HttpPatch("{id}")]
          [ProducesResponseType(typeof(Appointment), StatusCodes.Status402PaymentRequired)]
          [ProducesResponseType(StatusCodes.Status400BadRequest)]
          public async Task<IActionResult> Patch(int id, [FromBody] Appointment obj)
          {
              return Ok(200);
          }*/


    }
}
