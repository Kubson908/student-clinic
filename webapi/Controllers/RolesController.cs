using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Przychodnia.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        // /api/roles/create
        [HttpPost("create")]
        public async Task<IActionResult> CreateRoleAsync([FromBody] string name)
        {
            if (name == null) return BadRequest("Role is null");
            if (!_roleManager.RoleExistsAsync(name).GetAwaiter().GetResult())
            {
                await _roleManager.CreateAsync(new IdentityRole(name));

                return Ok("Added new role");
            }

            return BadRequest("Error");
        }

    }
}
