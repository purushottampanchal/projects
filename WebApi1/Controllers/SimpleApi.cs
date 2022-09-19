using Microsoft.AspNetCore.Mvc;
using WebApi1.Model;

namespace WebApi1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SimpleApi : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            Movie movie = new Movie()
            {
                Name = "Movie 1"
            };
            return "Movie = " + movie.Name;

        }

        [HttpGet("{id}", Name = "Get")]
        public String Get(int id)
        {
            return ("Get req with Id " + id);
        }

        // PUT api/<ProductController>/5
        [HttpPut]
        public string Put(int id, [FromBody] string Name)
        {

            return "Updated: "+id+" with new name"+Name;
        }

        [HttpPost]
        public String Post(String name)
        {
            return ("Movie added: " + name);
        }
    }
}