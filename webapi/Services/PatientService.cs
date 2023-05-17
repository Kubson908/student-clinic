using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Przychodnia.Shared;
using Przychodnia.Webapi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Przychodnia.Webapi.Services
{
    public class PatientService : IUserService
    {
        private readonly UserManager<Patient> _userManager;
        private readonly IConfiguration _configuration;

        public PatientService(UserManager<Patient> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
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
                Pesel = dto.Pesel
            };

            var result = await _userManager.CreateAsync(patient, dto.Password);

            if (result.Succeeded)
            {
                // TODO: Send a confirmation email

                return new UserManagerResponse
                {
                    Message = "Sukces",
                    IsSuccess = true
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
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Jakis klucz szyfrujacy"));

            var token = new JwtSecurityToken(
                issuer: "/localhost",
                audience: "/localhost",
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new UserManagerResponse
            {
                Message = "Zalogowano",
                IsSuccess = true,
                AccessToken = tokenString,
                ExpireDate = token.ValidTo
            };
        }
    }
}
