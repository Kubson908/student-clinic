using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using Przychodnia.Shared;
using Przychodnia.Webapi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Przychodnia.Webapi.Services
{
    public class PatientService : IUserService<RegisterDto, LoginDto>
    {
        private readonly UserManager<Patient> _userManager;
        private readonly IConfiguration _configuration;

        public PatientService(UserManager<Patient> userManager, 
            IConfiguration configuration, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        private async Task SendEmailConfirmationLink(Patient user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            UriBuilder baseUri = new UriBuilder("localhost:8080/auth/email-confirm");
            baseUri.Query = "query=" + token + "&id=" + user.Id;

            string mailFrom = "kartinghappywheels@gmail.com";
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Przychodnia Studencka", mailFrom));
            message.To.Add(new MailboxAddress(user.Email, user.Email));
            message.Subject = "Weryfikacja adresu email do konta w przychodni";
            var body = new BodyBuilder();
            body.HtmlBody = "<h3>Link do weryfikacji</h3>" + "<a href =http://" + baseUri + ">" + baseUri + "</a>";
            message.Body = body.ToMessageBody();
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate(mailFrom, "affpkqulzykvaima");
                client.Send(message);
                client.Disconnect(true);
            }
        }

        public async Task<UserManagerResponse> RegisterUserAsync(RegisterDto dto)
        {
            if (dto == null)
                throw new NullReferenceException("Należy wypełnić wymagane pola");

            if (dto.Password != dto.ConfirmPassword)
                return new UserManagerResponse
                {
                    Message = "Hasła nie są zgodne",
                    IsSuccess = false,
                };

            var patient = new Patient
            {
                Email = dto.Email,
                UserName = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                DateOfBirth = dto.DateOfBirth,
                Pesel = dto.Pesel,
                EmailConfirmed = false
            };

            var result = await _userManager.CreateAsync(patient, dto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(patient, "Patient");
                await SendEmailConfirmationLink(patient);

                return new UserManagerResponse
                {
                    Message = "Sukces",
                    IsSuccess = true,
                };
            }

            return new UserManagerResponse
            {
                Message = "Nie utworzono użytkownika",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description)
            };
        }

        public async Task<UserManagerResponse> LoginUserAsync(LoginDto dto)
        {
            if (dto == null)
                throw new NullReferenceException("Należy wypełnić wymagane pola");
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
                return new UserManagerResponse
                {
                    Message = "Brak użytkownika o podanym adresie Email",
                    IsSuccess = false
                };

            var confirmed = await _userManager.IsEmailConfirmedAsync(user);
            if (!confirmed)
                return new UserManagerResponse
                {
                    Message = dto.Email,
                    IsSuccess = false,
                    Errors = new List<string>() { "Not Confirmed" }
                };

            var result = await _userManager.CheckPasswordAsync(user, dto.Password);

            if (!result)
                return new UserManagerResponse
                {
                    Message = "Błędne hasło",
                    IsSuccess = false
                };

            var claims = new[]
            {
                new Claim("Email", dto.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Role, "Patient")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"] ?? "spare key"));

            var token = new JwtSecurityToken(
                issuer: _configuration["AuthSettings:Issuer"],
                audience: _configuration["AuthSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new UserManagerResponse
            {
                Message = "Zalogowano",
                IsSuccess = true,
                AccessToken = tokenString,
                ExpireDate = token.ValidTo,
                User = user.FirstName + " " + user.LastName,
                Roles = await _userManager.GetRolesAsync(user)
            };
        }
    }
}
