using jwtAuth.Models;
using jwtAuth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace jwtAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewAuthenticationController : ControllerBase
    {

        private AuthenticationService _auth ;
        private TokenValidator _tokenValidator ;

        public NewAuthenticationController(JwtConfig config)
        {
            _auth = new AuthenticationService(config);
            _tokenValidator = new TokenValidator(config);
        }

        [HttpGet("Test")]
        [Authorize]

        public async Task<IActionResult> test()
        {
            return Ok();
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {

            if(loginRequest == null)
            {
                return BadRequest("empty login credentials");
            }

            User user = _auth.LoginUser(loginRequest);
            
            if(user == null)
            {
                return Unauthorized("User Credentials Invalid");
            }

            SuccessfullAuthenticationResponce responce = _auth.GenerateLoginResponce(user);
            return Ok(responce);

        }


        [HttpPost("validate")]
        public IActionResult Validate(Token token)
        {
            return Ok(_tokenValidator.ValidateAccessToken(token.token));
        }


        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] Token refreshToken)
        {

            //validate refreshToken

            string tokenString = refreshToken.token;
            bool IsValidToken = _tokenValidator.ValidateRefreshToken(tokenString);
            if (!IsValidToken)
            {
                return BadRequest("Invalid Refresh token");
            }

            // get user by refresh token

            RefreshTokenDto refreshTokenDto = DemoRefreshDto.refreshTokenDtos.Where(x => x.RefreshToken == tokenString ).FirstOrDefault();
            if(refreshTokenDto == null)
            {
                return BadRequest("Invalid Refresh token");
            }

            //invalidate refresh token

            DemoRefreshDto.refreshTokenDtos.Remove(refreshTokenDto);

            //authenticate user 

            User user = DemoUsers.UsersRecords.FirstOrDefault(u => u.UserName == refreshTokenDto.UserName);
            if(user == null)
            {
                return BadRequest("User with this refresh token not found");
            }

            SuccessfullAuthenticationResponce responce = _auth.GenerateLoginResponce(user);

            return Ok(responce);

        }


        [Authorize]
        [HttpDelete("logout")]
        public async Task<IActionResult> Logout()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            string CurrentUserName = AuthenticationService.GetCurrentUserId(identity);

            if (string.IsNullOrEmpty(CurrentUserName))
            {
                return Unauthorized("User not logged in");
            }

            DemoRefreshDto.refreshTokenDtos.RemoveAll(o => o.UserName == CurrentUserName);

            return Ok("Logged out");

        }


    }
}
