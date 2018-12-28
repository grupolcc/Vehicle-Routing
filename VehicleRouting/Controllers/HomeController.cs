using System.Web.Mvc;

namespace VehicleRouting.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {

        [AllowAnonymous]
        public ActionResult Index()
        {
            this.ViewBag.Title = "Home Page";

            return this.View();
        }
    }
}
