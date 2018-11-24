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

            ViewModel localisationsModel = new ViewModel();
            localisationsModel.PointsOfDelivery = db.PointsOfDeliveries.ToList();
            localisationsModel.Vehicles = db.Vehicles.ToList();
            return this.View(localisationsModel);
        }    
    }
}