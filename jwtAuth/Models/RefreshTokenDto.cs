using System.ComponentModel.DataAnnotations;

namespace jwtAuth.Models
{

    public static class DemoRefreshDto
    {
        public static List<RefreshTokenDto> refreshTokenDtos = new List<RefreshTokenDto>();

    }

    public class RefreshTokenDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string RefreshToken { get; set; }

    }
}
