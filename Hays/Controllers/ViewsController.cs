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
    }
}
