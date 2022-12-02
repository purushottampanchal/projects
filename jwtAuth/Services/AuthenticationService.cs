using jwtAuth.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace jwtAuth.Services
{
    public class AuthenticationService
    {
        readonly JwtConfig _config;
        public AuthenticationService(JwtConfig config)
        {
            _config = config;
        }

        public User? LoginUser(LoginRequest userCredential)
        {

            //getting user's data from the userName
            var user = DemoUsers.UsersRecords.FirstOrDefault(u => u.UserName == userCredential.UserName);

            //if user is null then no user is found of given username

            if (user != null)
                if (VerifyPsswordHashes(user, userCredential.Password))
                {
                    return user;
                }

            return null;


        }

        private static bool VerifyPsswordHashes(User user, string password)
        {
            var result = DemoUsers.pw.VerifyHashedPassword(user.UserName, user.HashedPassword, password);
            return result == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Success;

        }

        public SuccessfullAuthenticationResponce GenerateLoginResponce(User user)
        {
            string token = GenerateToken(user);
            string refreshToken = GenerateRefreshToke();

            //get from db
            DemoRefreshDto.refreshTokenDtos.Add(new RefreshTokenDto()
            {
                RefreshToken = refreshToken,
                UserName = user.UserName
            });

            //=------

            return new SuccessfullAuthenticationResponce()
            {
                AccessToken = token,
                RefreshToken = refreshToken
            };

        }

        private string GenerateRefreshToke()
        { 
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.RefreshTokenSecretKey));

            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _config.Issuer,
                audience: _config.Audience,
                claims: null,
                notBefore: DateTime.UtcNow,
                expires: DateTime.Now.AddMinutes(_config.RefreshTokenExpirationMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateToken(User user)
        {

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier , user.Id.ToString()),
                new Claim(ClaimTypes.GivenName, user.UserName),
                new Claim(ClaimTypes.Role, user.Role),

            };

            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.AccessTokenSecretKey));

            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _config.Issuer,
                audience: _config.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.Now.AddMinutes(_config.AccessTokenExpirationMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);


        }

        public static string? GetCurrentUserId(ClaimsIdentity identity)
        {

            if (identity != null)
            {
                var id =
                    identity.Claims.FirstOrDefault(i => i.Type == ClaimTypes.NameIdentifier)?.Value;

                return id;
            }
            return null;

        }

    }
}
