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
            MenuList = _db.MenuItems;
            ViewData["MenuList"] = MenuList;
            ViewData["User"] = u;
            Console.WriteLine("Index(0_)");
            return View();
        }

        public IActionResult Login()
        {
            
            return View();
        }

        [HttpPost]
        public IActionResult Login(User u)
        {
            int id = ValidateUser(u);
            if (id == -1)
            {   //user not found
                ViewData["login-message"] = "Error: Invalid Credentials";
                return View();

            }
            else
            {
                u.Id = id;
                //Console.WriteLine("----------------id:"+u.Id);
                Console.WriteLine("----------------Name:"+u.Name);
                TempData["userId"] = u.Id;
                TempData["userName"] = u.Name;
                
                return RedirectToAction("Index");
            }
        }
        

        private int ValidateUser(User u)
        {

            userList = _db.Users;

            foreach (User user in userList)
            {
                if(user.Name == u.Name && user.Password == u.Password)
                {
                    return user.Id;
                }
            }
            return -1;
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

            userList = _db.Users;

            var qqws = userList.FirstOrDefault(x => x.Name == Name);

            if (string.IsNullOrEmpty(Name))
            {
                ViewData["ErrorMessage"] = "Name/User ID Cannot be empty";
                return View("Register");
            }
            else if (userList.FirstOrDefault(x => x.Name == Name) != null) 
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


        public IActionResult DisplaySingleProduct(int id)
        {

            MenuItem menuItem = MenuList.FirstOrDefault(x => x.Id == id);
            if (menuItem != null)
            {
                return View(menuItem);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult PurOrderFor(IFormCollection col)
        {
            if (col == null)
            {
                return Content("Error occured: !!!!");
            }
            else
            {
                MenuItem menuItem = MenuList.FirstOrDefault(x => x.Id == col["id"]);

                ViewData["order-qty"] = col["qty"];
                ViewData["order-id"] = col["id"];
                ViewData["order-id"] = col["address"];

                OrderItem order = new OrderItem()
                {
                    /*Address = col["address"],
                    Cost = menuItem.Cost * Convert.ToInt32(col["qty"]),
                    Name = menuItem.Name,
                    OrderStatus = OrderItem.Order_put,
                    Qty = Convert.ToInt32(col["qty"]),
                    UserName =*/

                };
                return RedirectToAction("Order");
            }

            //  return Content(col["id"] + "|" + col["qty"]);
        }

        public IActionResult Order()
        {
            return View();
        }


        public IActionResult Logout()
        {
            return RedirectToAction("Index");
        }

        //get {id = userId}
        //userName in order is user's Id
        public IActionResult DisplayOrders(int id)
        {
            if(id == 0)
            {
                ViewData["Message"] = "Please Log in to see orders";
                return View("DisplayError");
            }
            var orderlist = _db.Orders.Where(x => x.UserName == id.ToString()).ToList();
            return View(orderlist);
        }

        public IActionResult CancelOrder(int id)
        {

            OrderItem order = _db.Orders.Where(x => x.Id == id).FirstOrDefault();
             
            if(order == null)
            {
                ViewData["Message"] = "Error: Unable to find order with id :" + id;
                return View("DisplayError");
            }
            else
            {
                order.OrderStatus = OrderItem.Order_cancelled;

                _db.Orders.Where(x => x.Id == id).FirstOrDefault().OrderStatus = OrderItem.Order_cancelled;
                _db.SaveChanges();  

                return Redirect("DisplayOrders?id=" + order.UserName);
            }

        }

    }
}