using Microsoft.AspNetCore.Mvc;

namespace DB_Test.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
