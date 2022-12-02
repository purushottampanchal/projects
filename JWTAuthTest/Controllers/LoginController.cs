using JWTAuthTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTAuthTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        IConfiguration configuration;

        public LoginController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IActionResult Login(UserLogin ForUser)
        {

            var CurrentUser = AuthenticateUser(ForUser);

            if (CurrentUser != null)
            {
                var token = GenerateTOken(CurrentUser);

                return Ok(token);

            }
            return BadRequest("User not found");
        }

        private string GenerateTOken(User forUser)
        {
            var SecKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var Credentials = new SigningCredentials(SecKey, SecurityAlgorithms.HmacSha256);

            var Claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, forUser.UserName),
                new Claim(ClaimTypes.Role, forUser.Role)
            };

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["jwt:Audience"],
                claims: Claims,
                signingCredentials: Credentials,
                expires: DateTime.Now.AddHours(2)
                );

            //return token;
            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        private User AuthenticateUser(UserLogin forUser)
        {

            var CurrUser = UserConstants.users.FirstOrDefault(
                o => o.UserName == forUser.UserName && o.Password == forUser.Password
                );

            return CurrUser;

        }
    }
}
