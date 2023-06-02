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

        public async Task SendEmailConfirmationCode(Patient user)
        {
            var token = await _userManager.GenerateUserTokenAsync(user, "EmailConfirmationTokenProvider", UserManager<object>.ConfirmEmailTokenPurpose);

            string mailFrom = "przychodniaspaghettimafia@gmail.com";
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Przychodnia Studencka", mailFrom));
            message.To.Add(new MailboxAddress(user.Email, user.Email));
            message.Subject = "Weryfikacja adresu email do konta w przychodni";
            var body = new BodyBuilder();
            body.HtmlBody = "<h3>Kod do weryfikacji</h3>" + "<h1>" + token + "</h1>";
            message.Body = body.ToMessageBody();
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate(mailFrom, "fegdvxpcrnsjwlvn");
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
                await SendEmailConfirmationCode(patient);

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

        public async Task<UserManagerResponse> DeleteUserAsync(string userId)
        {
            if (userId == null)
                return new UserManagerResponse
                {
                    Message = "User id us null",
                    IsSuccess = false
                };
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return new UserManagerResponse
                {
                    Message = "User not found",
                    IsSuccess = false
                };
            await _userManager.DeleteAsync(user);

            return new UserManagerResponse
            {
                Message = "User has been deleted",
                IsSuccess = true
            };
        }
    }
}
