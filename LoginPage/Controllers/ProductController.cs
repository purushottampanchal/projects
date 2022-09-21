using LoginPage.Data;
using LoginPage.Models;
using Microsoft.AspNetCore.Mvc;

namespace LoginPage.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        IEnumerable<MenuItem> MenuList;

        public ProductController(ApplicationDbContext db)
        {
            _db = db;
            MenuList = _db.MenuItems;
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
        public IActionResult DisplaySingleProduct(string id)
        {
            MenuItem menuItem = MenuList.FirstOrDefault(x => x.Id.ToString() == id);
            if (menuItem != null)
            {
                return View(menuItem);
            }
            return View("Views/Home");
        }
    }
}
