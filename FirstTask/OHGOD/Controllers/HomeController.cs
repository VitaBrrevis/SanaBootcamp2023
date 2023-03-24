using Microsoft.AspNetCore.Mvc;
using OHGOD.Models;
using System.Diagnostics;

namespace OHGOD.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public JsonResult Greeting(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Json(new { name = "There is no one to say hi to(" });
            }

            var greeting = "Hello, " + name + "!";
            return Json(new { name = greeting });
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}