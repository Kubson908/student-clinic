using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
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

        string hashPassword(string password)
        {
            var sha = SHA256.Create();
            var asByteArray = Encoding.Default.GetBytes(password);
            var hashedPassword = sha.ComputeHash(asByteArray);
            return Convert.ToBase64String(hashedPassword);
        }

        [HttpGet]
        [ProducesResponseType(typeof(Patient), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IEnumerable<Patient>> Get() => await _db.Patients.ToListAsync();

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
