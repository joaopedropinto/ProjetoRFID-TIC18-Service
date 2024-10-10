using Microsoft.AspNetCore.Mvc;

namespace Portal_WebAPI.WebAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Portal RFID";

            return View();
        }
    }
}
