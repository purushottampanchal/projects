using LoginPage.Data;
using LoginPage.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LoginPage.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        IEnumerable<User> userList;
        IEnumerable<MenuItem> MenuList;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Index()
        {
            /*_db.Users.Add(new User()
            {
                
                Name = "User",
                Password = "1"
            });
            _db.SaveChanges();*/

            MenuList = _db.MenuItems;
            ViewData["MenuList"] = MenuList; 
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

            userList = _db.Users;

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

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterUser(string Name, string Password, string cnfPassword)
        {
            if (string.IsNullOrEmpty(Name))
            {
                ViewData["ErrorMessage"] = "Name/User ID Cannot be empty";
                return View("Register");
            }
            else if ((userList.Where(u => u.Name.Equals(Name))) != null)
            {
                ViewData["ErrorMessage"] = "Name/User Already Exist";
                return View("Register");
            }
            else if (string.IsNullOrEmpty(Password))
            {
                ViewData["ErrorMessage"] = "Password cannot be empty";
                return View("Register");
            }
            else if(string.IsNullOrEmpty(cnfPassword) || !cnfPassword.Equals(Password))
            {
                ViewData["ErrorMessage"] = "Confirm password doesnt match";
                return View("Register");
            }
            else
            {
                User u = new User()
                {
                    Name = Name,
                    Password = Password
                };
                _db.Users.Add(u);   
                _db.SaveChanges();

                ViewData["Greeting1"] = "Log In For";
                ViewData["Greeting2"] = " Better Experince";

                TempData["message"] = "Account Created Successfully";
                return RedirectToAction("Login");  
            }
        }

    }
}