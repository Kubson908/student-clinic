using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Przychodnia.Shared;
using Przychodnia.Webapi.Services;
using System.Net.Mail;
using MimeKit;
using Microsoft.AspNetCore.Identity;
using Przychodnia.Webapi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Przychodnia.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUserService<RegisterDto, LoginDto> _patientService;
        private IUserService<RegisterEmployeeDto, LoginDto> _employeeService;
        private UserManager<Patient> _patientManager; 

        public AuthController(IUserService<RegisterDto, LoginDto> patientService, 
            IUserService<RegisterEmployeeDto, LoginDto> employeeService,
            UserManager<Patient> patientManager)
        {
            _patientService = patientService;
            _employeeService = employeeService;
            _patientManager = patientManager;
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

        [HttpPost("Send-Reset-Link")]
        public async Task<IActionResult> SendResetPasswordLink([FromBody]string email)
        {
            if (email == null) return BadRequest("Email is null");
            var user = await _patientManager.FindByEmailAsync(email);
            if (user == null) return BadRequest("User not found");

            var token = await _patientManager.GeneratePasswordResetTokenAsync(user);

            UriBuilder baseUri = new UriBuilder("localhost:8080/auth/password-reset");
            baseUri.Query = "query=" + token + "&id=" + user.Id;

            string mailFrom = "kartinghappywheels@gmail.com";
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Przychodnia Studencka", mailFrom));
            message.To.Add(new MailboxAddress(email, email));
            message.Subject = "Reset hasła do konta w przychodni";
            var body = new BodyBuilder();
            body.HtmlBody = "<h3>Link do zmiany hasła</h3>" + "<a href =http://" + baseUri + ">" + baseUri + "</a>";
            message.Body = body.ToMessageBody();
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate(mailFrom, "affpkqulzykvaima");
                client.Send(message);
                client.Disconnect(true);
            }

            return Ok("Email sent");

        }

        [HttpPost("Reset-Password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            if (dto.Password == null) return BadRequest("Password is null");

            var user = await _patientManager.FindByIdAsync(dto.Id);
            if (user == null) return NotFound("User not found");
            var result = await _patientManager.ResetPasswordAsync(user, dto.Token, dto.Password);
            if (result.Succeeded) return Ok("Success");
            return BadRequest("Token is invalid");
        }
    }
}
