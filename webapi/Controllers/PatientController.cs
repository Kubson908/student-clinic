using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Przychodnia.Webapi.Data;
using Przychodnia.Webapi.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Przychodnia.Shared;

namespace Przychodnia.Webapi.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class PatientController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<Patient> _patientManager;
        public PatientController(ApplicationDbContext db, UserManager<Patient> patientManager)
        {
            _db = db;
            _patientManager = patientManager;
        }

        [Authorize(Roles = "Employee, Staff")]
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
        public async Task<IActionResult> GetPatientCard([FromRoute] string? id)
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
                    p.Verified,
                    TreatmentHistory = p.Appointments.Where(a => a.Finished).Select(a => new
                    {
                        a.Id,
                        a.Date,
                        a.Specialization,
                    })
                }).FirstOrDefaultAsync();
            return patient == null ? NotFound("Patient not found") : Ok(patient);
        }

        [Authorize(Roles = "Staff, Patient")]
        [HttpPatch("update")]
        [HttpPatch("update/{patientId}")]
        public async Task<IActionResult> UpdatePatient([FromRoute] string? patientId, [FromBody] UpdatePatientDto dto)
        {
            string id;
            string? role = HttpContext.User.FindFirstValue(ClaimTypes.Role);
            if (role != null && role.Contains("Patient")) id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
            else if (patientId != null) id = patientId;
            else return BadRequest("Patient id is null");
            var patient = await _patientManager.FindByIdAsync(id);
            if (patient == null) return NotFound("Patient not found");
            foreach (var prop in typeof(Patient).GetProperties())
            {
                var fromProp = typeof(UpdatePatientDto).GetProperty(prop.Name);
                var toValue = fromProp != null ? fromProp.GetValue(dto, null) : null;
                if (toValue != null)
                {
                    prop.SetValue(patient, toValue, null);
                }
            }
            foreach (var prop in typeof(IdentityUser).GetProperties())
            {
                var fromProp = typeof(UpdatePatientDto).GetProperty(prop.Name);
                var toValue = fromProp != null ? fromProp.GetValue(dto, null) : null;
                if (toValue != null)
                {
                    prop.SetValue(patient, toValue, null);
                }
            }
            var result = await _patientManager.UpdateAsync(patient);
            if (!result.Succeeded) return BadRequest("Error");
            return Ok("Patient updated");
        }
    }
}
