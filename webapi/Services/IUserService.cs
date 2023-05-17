using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Przychodnia.Shared;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Przychodnia.Webapi.Services
{
    public interface IUserService<R, L>
    {
        Task<UserManagerResponse> RegisterUserAsync(R dto);
        Task<UserManagerResponse> LoginUserAsync(L dto);
    }

    public class UserService : IUserService<RegisterDto, LoginDto>
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public UserService(UserManager<IdentityUser> userManager, IConfiguration configuration)
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

            var identityUser = new IdentityUser
            {
                Email = dto.Email,
                UserName = dto.Email,
            };

            var result = await _userManager.CreateAsync(identityUser, dto.Password);

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

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]));

            var token = new JwtSecurityToken(
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