using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Przychodnia.Webapi.Data;
using Przychodnia.Webapi.Models;
using Microsoft.AspNetCore.Authorization;

namespace Przychodnia.Webapi.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Employee")]
    public class PatientController : Controller
    {
        private readonly ApplicationDbContext _db;
        public PatientController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Patient), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IEnumerable<object>> Get()
        {
            var result = await _db.Patients.ToListAsync();
            var patients = result.Select(p => new
            {
                p.Id,
                p.FirstName,
                p.LastName,
                p.DateOfBirth,
                p.Pesel,
                p.PhoneNumber,
                p.Email,
                p.Appointments
            });
            return patients;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Patient), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById(int id)
        {
            var patient = await _db.Patients.FindAsync(id);
            return patient == null ? NotFound() : Ok(patient);
        }
    }
}
