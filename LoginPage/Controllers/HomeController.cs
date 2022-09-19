using LoginPage.Data;
using LoginPage.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LoginPage.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Index()
        {
            _db.Users.Add(new User()
            {
                
                Name = "User",
                Password = "1"
            });
            _db.SaveChanges();

            return View();
        }

        public IActionResult Index(User u)
        {
            return View(u);
        }

        public IActionResult Login()
        {
            
            return View();
        }

        [HttpPost]
        public IActionResult Login(User u)
        {
            if (ValidateUser(u))
                return View("Index", u);
            else
            {
                ViewData["Message"] = "Error: Invalid Credentials";
                return View(u);
            }
        }

        private bool ValidateUser(User u)
        {
            IEnumerable<User> userList = _db.Users;

            foreach (User user in userList)
            {
                if(user.Name == u.Name && user.Password == u.Password)
                {
                    return true;
                }
            }
            return false;
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}