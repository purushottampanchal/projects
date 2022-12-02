using LoginApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LoginApi.Controllers
{
    public class LoginController : Controller
    {
        public async Task<IActionResult> AttemptLogin(IFormCollection collection)
        {
            var login = new LoginRequest()
            {
                UserName = collection["username"],
                Password = collection["password"]
            };
            HttpClient client = new HttpClient();
            JsonContent content = JsonContent.Create(login);
            var res = await client.PostAsync("https://localhost:7228/api/Authentication/login", content);

            switch (res.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    TempData["msg"] = "logged In";
                    break;
                case System.Net.HttpStatusCode.BadRequest:
                    TempData["msg"] = "bad req";
                    break;
                case System.Net.HttpStatusCode.Unauthorized:
                    TempData["msg"] = "invalid credentials";
                    break;
                case System.Net.HttpStatusCode.InternalServerError:
                    TempData["msg"] = "invternal server error";
                    break;
            }
            var result = await res.Content.ReadAsStringAsync();

            Console.WriteLine(result);

            return RedirectToAction("Login", "Home");

        }
    }
}
