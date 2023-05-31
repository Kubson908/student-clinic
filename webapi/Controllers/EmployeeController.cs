using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Przychodnia.Webapi.Data;
using Przychodnia.Webapi.Models;
using Przychodnia.Shared;
using Microsoft.AspNetCore.Authorization;

namespace Przychodnia.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<Employee> _userManager;
        
        public EmployeeController(ApplicationDbContext db, UserManager<Employee> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Employee>>> Get()
        {
            var employees = await _db.Employees.Select(e => new
            {
                e.Id,
                e.FirstName,
                e.LastName,
                e.Specialization,
            }).ToListAsync();
            return Ok(employees);
        }

        [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var Worker = await _db.Employees.Select(e => new
            {
                e.Id,
                e.FirstName,
                e.LastName,
                e.Specialization,
                e.Email,
                e.PhoneNumber,
                e.DateOfBirth,
                e.Pesel
            }).FirstOrDefaultAsync(e => e.Id == id);
            return Worker == null ? NotFound("Employee not found") : Ok(Worker);
        }

        [HttpPatch("update/{id}")]
        public async Task<IActionResult> Update([FromRoute]string id, [FromBody]UpdateEmployeeDto dto)
        {
            if (dto == null) return BadRequest("Model is null");
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null) return BadRequest("User not found");
                user.Email = dto.Email;
                user.NormalizedEmail = dto.Email.Normalize();
                user.UserName = dto.Email;
                user.NormalizedEmail = dto.Email.Normalize();
                user.FirstName = dto.FirstName;
                user.LastName = dto.LastName;
                user.PhoneNumber = dto.PhoneNumber;
                user.DateOfBirth = dto.DateOfBirth;
                user.Specialization = dto.Specialization;
                user.Pesel = dto.Pesel;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded) return Ok("Updated");

                return BadRequest("Error");
            }
            return BadRequest("Model is not valid");
        }

        // TODO: sprawdzanie dostępności lekarza o konkretnej godzinie (przypisywanie wizyt)
        [Authorize(Roles = "Staff")]
        [HttpGet("available-doctors")]
        public async Task<IActionResult> GetAvailableDoctors([FromQuery] DateTime date, [FromQuery] Specialization specialization)
        {
            if (date < DateTime.Now || date.Equals(null)) return BadRequest("Model error");

            var doctors = await _db.Employees.Where(e => e.Specialization == specialization 
                && !e.Appointments.Select(a => a.Date).Contains(date))
                .Select(d => new
                {
                    d.Id,
                    d.FirstName,
                    d.LastName,
                }).ToListAsync();

            /*var test = _db.Employees.Select(e => e.Appointments.Select(a => a.Date));*/
            return Ok(doctors);
        }
    }
}
