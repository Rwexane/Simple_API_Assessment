using Microsoft.AspNetCore.Mvc;

namespace Simple_API_Assessment.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
