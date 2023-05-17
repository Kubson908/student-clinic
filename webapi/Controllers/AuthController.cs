using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Przychodnia.Shared;
using Przychodnia.Webapi.Services;

namespace Przychodnia.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUserService _patientService;

        public AuthController(IUserService patientService)
        {
            _patientService = patientService;
        }

        // /api/auth/register
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody]RegisterDto model)
        {
            if(ModelState.IsValid)
            {
                var result = await _patientService.RegisterUserAsync(model);

                if (result.IsSuccess) return Ok("Sukces"); // Status code: 200

                return BadRequest(result);
            }

            return BadRequest("Pola nie są poprawne"); // Status code: 400
        }

        // /api/auth/login
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody]LoginDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _patientService.LoginUserAsync(model);

                if (result.IsSuccess)
                    return Ok(result);

                return BadRequest("Błąd logowania");
            }

            return BadRequest("Pola nie są poprawne");
        }
    }
}
