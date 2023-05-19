using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Przychodnia.Webapi.Data;
using Przychodnia.Webapi.Models;
using Przychodnia.Shared;

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
        public async Task<IEnumerable<Employee>> Get() => await _db.Employees.ToListAsync();

        [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var Worker = await _db.Employees.FindAsync(id);
            return Worker == null ? NotFound() : Ok(Worker);
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

    }
}
