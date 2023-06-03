using Microsoft.AspNetCore.Mvc;
using Przychodnia.Shared;
using Przychodnia.Webapi.Services;
using System.Net.Mail;
using MimeKit;
using Microsoft.AspNetCore.Identity;
using Przychodnia.Webapi.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Przychodnia.Webapi.CustomTokenProviders;
using System.Security.Claims;

namespace Przychodnia.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUserService<RegisterDto, LoginDto> _patientService;
        private IUserService<RegisterEmployeeDto, LoginDto> _employeeService;
        private UserManager<Patient> _patientManager;
        private UserManager<Employee> _employeeManager;

        public AuthController(IUserService<RegisterDto, LoginDto> patientService,
            IUserService<RegisterEmployeeDto, LoginDto> employeeService,
            UserManager<Patient> patientManager, UserManager<Employee> employeeManager)
        {
            _patientService = patientService;
            _employeeService = employeeService;
            _patientManager = patientManager;
            _employeeManager = employeeManager;
        }

        // /api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _patientService.RegisterUserAsync(model);

                if (result.IsSuccess) return Ok("Succes"); // Status code: 200

                return BadRequest(result);
            }

            return BadRequest("Invalid data"); // Status code: 400
        }
        // dodać do vue lub przerobić na sms code wysyłany na maila
        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailDto dto)
        {
            if (dto.Token == null) return BadRequest("Code is null");

            var user = await _patientManager.FindByEmailAsync(dto.Email);
            if (user == null) return NotFound("User not found");
            var result = await _patientManager.VerifyUserTokenAsync(user, "EmailConfirmationTokenProvider", UserManager<object>.ConfirmEmailTokenPurpose, dto.Token);
            if (result)
            {
                string token = await _patientManager.GenerateEmailConfirmationTokenAsync(user);
                await _patientManager.ConfirmEmailAsync(user, token);
                return Ok("Email confirmed");
            }
            return BadRequest("Invalid code");
        }

        [HttpPost("resend-email")]
        public async Task<IActionResult> ResendEmail([FromBody] ConfirmEmailDto dto)
        {
            var user = await _patientManager.FindByEmailAsync(dto.Email);
            if (user == null) return NotFound("User not found");
            await _patientService.SendEmailConfirmationCode(user);
            return Ok("Mail sent");
        }

        // /api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto model)
        {
            if (ModelState.IsValid)
            {
                var domain = new MailAddress(model.Email).Host;

                var result = new UserManagerResponse();

                if (domain == "przychodnia.com") result = await _employeeService.LoginUserAsync(model);
                else result = await _patientService.LoginUserAsync(model);

                if (result.IsSuccess)
                    return Ok(result);
                if (!result.Errors.IsNullOrEmpty())
                {
                    await ResendEmail(new ConfirmEmailDto { Email = model.Email });
                    return StatusCode(403, "Email not confirmed");
                }


                return Unauthorized("Login error");
            }

            return BadRequest("Invalid data");
        }

        // /api/auth/register-employee
        [HttpPost("register-employee")]
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
        [HttpPost("login-employee")]
        public async Task<IActionResult> LoginEmployeeAsync([FromBody] LoginDto model)
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

        [HttpPost("send-reset-link")]
        public async Task<IActionResult> SendResetPasswordLink([FromBody] ConfirmEmailDto dto)
        {
            if (dto.Email == null) return BadRequest("Email is null");
            var user = await _patientManager.FindByEmailAsync(dto.Email);
            if (user == null) return BadRequest("User not found");

            var token = await _patientManager.GeneratePasswordResetTokenAsync(user);

            UriBuilder baseUri = new UriBuilder("localhost:8080/auth/password-reset");
            baseUri.Query = "token=" + token + "&id=" + user.Id;

            string mailFrom = "przychodniaspaghettimafia@gmail.com";
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Przychodnia Studencka", mailFrom));
            message.To.Add(new MailboxAddress(user.FirstName, dto.Email));
            message.Subject = "Reset hasła do konta w przychodni";
            var body = new BodyBuilder();
            body.HtmlBody = "<h3>Link do zmiany hasła</h3>" + "<a href =http://" + baseUri + ">" + baseUri + "</a>";
            message.Body = body.ToMessageBody();
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate(mailFrom, "fegdvxpcrnsjwlvn");
                client.Send(message);
                client.Disconnect(true);
            }

            return Ok("Email sent");
        }

        [HttpPatch("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            if (dto.Password == null ) return BadRequest("Password is null");

            var user = await _patientManager.FindByIdAsync(dto.Id);
            if (user == null) return NotFound("User not found");
            var token = dto.Token.Replace(" ", "+");
            var result = await _patientManager.ResetPasswordAsync(user, token, dto.Password);
            Console.WriteLine(dto.Token.ToString() + "\n" + token);
            if (result.Succeeded) return Ok("Success");
            return BadRequest("Token is invalid");
        }

        [Authorize(Roles = "Staff")]
        [HttpPatch("employee-reset-password")]
        public async Task<IActionResult> ResetEmployeePassword([FromBody] ResetPasswordDto dto)
        {
            if (dto.Password == null) return BadRequest("Password is null");

            var user = await _employeeManager.FindByIdAsync(dto.Id);
            if (user == null) return NotFound("User not found");
            var token = await _employeeManager.GeneratePasswordResetTokenAsync(user);
            var result = await _employeeManager.ResetPasswordAsync(user, token, dto.Password);
            if (result.Succeeded) return Ok("Success");
            return BadRequest("Token is invalid");
        }

        [HttpPatch("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            if (dto.CurrentPassword == dto.NewPassword) return Conflict("New password cannot be the same as current password");
            string ? role = HttpContext.User.FindFirstValue(ClaimTypes.Role);
            string? id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id == null || role == null) return BadRequest("Error");
            IdentityResult result;
            if (role.Contains("Patient"))
            {
                var user = await _patientManager.FindByIdAsync(id);
                result = await _patientManager.ChangePasswordAsync(user!, dto.CurrentPassword, dto.NewPassword);
            }
            else 
            {
                var user = await _employeeManager.FindByIdAsync(id);
                result = await _employeeManager.ChangePasswordAsync(user!, dto.CurrentPassword, dto.NewPassword);
            }
            Console.WriteLine(result.Succeeded);
            if (result.Succeeded) return Ok("Password changed");
            return BadRequest("Cannot change password");
        }

        [HttpPatch("delete-patient")]
        public async Task<IActionResult> DeletePatient([FromBody] string patientId)
        {
            var result = await _patientService.DeleteUserAsync(patientId);

            if (result.IsSuccess) return Ok("Succes"); // Status code: 200

            return BadRequest(result);
        }
    }
}
