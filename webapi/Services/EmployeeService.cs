using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Przychodnia.Shared;
using Przychodnia.Webapi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Przychodnia.Webapi.Services
{
    public class EmployeeService : IUserService<RegisterEmployeeDto, LoginDto>
    {
        private readonly UserManager<Employee> _userManager;
        private readonly IConfiguration _configuration;

        public EmployeeService(UserManager<Employee> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<UserManagerResponse> RegisterUserAsync(RegisterEmployeeDto dto)
        {
            if (dto == null)
                throw new NullReferenceException("Należy wypełnić wymagane pola");

            if (dto.Password != dto.ConfirmPassword)
                return new UserManagerResponse
                {
                    Message = "Hasła nie są zgodne",
                    IsSuccess = false,
                };

            var employee = new Employee
            {
                Email = dto.Email,
                UserName = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                DateOfBirth = dto.DateOfBirth,
                Pesel = dto.Pesel,
                Specialization = dto.Specialization,
                EmailConfirmed = true,
            };

            var result = await _userManager.CreateAsync(employee, dto.Password);

            if (result.Succeeded)
            {
                if (dto.Specialization == null) await _userManager.AddToRoleAsync(employee, "Staff");
                await _userManager.AddToRoleAsync(employee, "Employee");
                // TODO: Send a confirmation email

                return new UserManagerResponse
                {
                    Message = "Sukces",
                    IsSuccess = true
                };
            }

            return new UserManagerResponse
            {
                Message = "Nie utworzono pracownika",
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
                new Claim(ClaimTypes.Role, "Employee"),
                user.Specialization == null ? new Claim(ClaimTypes.Role, "Staff") : null
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"] ?? "spare key"));

            var token = new JwtSecurityToken(
                issuer: _configuration["AuthSettings:Issuer"],
                audience: _configuration["AuthSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(30),
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
