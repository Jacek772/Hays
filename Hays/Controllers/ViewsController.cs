using Microsoft.AspNetCore.Mvc;

namespace Hays.Controllers
{
    [Route("/views")]
    public class ViewsController : Controller
    {
        [HttpGet("home")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet("login")]
        public ActionResult Login()
        {
            return View();
        }

        [HttpGet("register")]
        public ActionResult Register()
        {
            return View();
        }

        [HttpGet("dashboard")]
        public ActionResult Dashboard()
        {
            return View();
        }

        [HttpGet("settings")]
        public ActionResult Settings()
        {
            return View();
        }
    }
}
