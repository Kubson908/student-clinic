using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Przychodnia.Webapi.Data;
using Przychodnia.Webapi.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Przychodnia.Webapi.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
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

        [Authorize(Roles = "Staff, Employee, Patient")]
        [HttpGet("patient-card/{id}")]
        [HttpGet("patient-card")]
        [ProducesResponseType(typeof(Patient), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPatientCard([FromRoute] string? id )
        {
            string? role = HttpContext.User.FindFirstValue(ClaimTypes.Role);
            string patientId = string.Empty;
            if (role != null && role.Contains("Patient")) patientId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            else if (role == "Employee" && id != null) patientId = id;
            if (patientId == string.Empty) return StatusCode(418);
            var patient = await _db.Patients.Where(p => p.Id == patientId).Include(p =>  p.Appointments)
                .Select(p => new
                {
                    p.FirstName,
                    p.LastName,
                    p.PhoneNumber,
                    p.Email,
                    p.Pesel,
                    p.DateOfBirth,
                    p.Allergies,
                    p.Medicines,
                    TreatmentHistory = p.Appointments.Where(a => a.Finished).Select(a => new
                    {
                        a.Id,
                        a.Date,
                        a.Specialization,
                    })
                }).FirstOrDefaultAsync();
            return patient == null ? NotFound("Patient not found") : Ok(patient);
        }
    }
}
