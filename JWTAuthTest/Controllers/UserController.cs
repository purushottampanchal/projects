using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTAuthTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {



        [HttpGet("Admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminEndPoint()
        {
           
            return Ok($"Hello Admin");

        }

        [HttpGet("Trainer")]
        [Authorize(Roles = "Trainer")]
        public IActionResult TrainerEndPoint()
        {
           
            return Ok($"Hello Trainer");

        }


        [HttpGet("Index")]
        public IActionResult Index()
        {
            return Ok("You are on index page / public end point");
        }

    }
}
