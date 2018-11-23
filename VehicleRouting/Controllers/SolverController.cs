using System.Linq;
using System.Web.Mvc;
using VehicleRouting.Models;

namespace VehicleRouting.Controllers
{
    public class SolverController : Controller
    {
        private VehicleDbContext db = new VehicleDbContext();

        public ActionResult Index()
        {
            this.ViewBag.Title = "Application";

            return this.View(this.db.PointsOfDeliveries.ToList());
        }        
    }
}