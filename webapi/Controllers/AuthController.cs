using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Przychodnia.Shared;
using Przychodnia.Webapi.Services;
using System.Net.Mail;

namespace Przychodnia.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUserService<RegisterDto, LoginDto> _patientService;
        private IUserService<RegisterEmployeeDto, LoginDto> _employeeService;

        public AuthController(IUserService<RegisterDto, LoginDto> patientService, 
            IUserService<RegisterEmployeeDto, LoginDto> employeeService)
        {
            _patientService = patientService;
            _employeeService = employeeService;
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
                var domain = new MailAddress(model.Email).Host;

                var result = new UserManagerResponse();

                if (domain == "przychodnia.com") result = await _employeeService.LoginUserAsync(model);
                else result = await _patientService.LoginUserAsync(model);

                if (result.IsSuccess)
                    return Ok(result);

                return BadRequest("Błąd logowania");
            }

            return BadRequest("Pola nie są poprawne");
        }

        // /api/auth/register-employee
        [HttpPost("Register-Employee")]
        public async Task<IActionResult> RegisterEmployeeAsync([FromBody] RegisterEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _employeeService.RegisterUserAsync(model);

                if (result.IsSuccess) return Ok("Sukces"); // Status code: 200

                return BadRequest(result);
            }

            return BadRequest("Pola nie są poprawne"); // Status code: 400
        }

        // /api/auth/login-employee
        [HttpPost("Login-Employee")]
        public async Task<IActionResult> LoginEmployeeAsync([FromBody]LoginDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _employeeService.LoginUserAsync(model);

                if (result.IsSuccess)
                    return Ok(result);

                return BadRequest("Błąd logowania");
            }

            return BadRequest("Pola nie są poprawne");
        }
    }
}
