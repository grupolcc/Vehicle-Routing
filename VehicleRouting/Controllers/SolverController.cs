using System.Linq;
using System.Web.Mvc;
using VehicleRouting.Models;

namespace VehicleRouting.Controllers
{
    public class SolverController : Controller
    {
        private readonly VehicleDbContext db = new VehicleDbContext();

        public ActionResult Index()
        {
            this.ViewBag.Title = "Application";

            LocationsViewModel locationsModel = new LocationsViewModel
            {
                PointsOfDelivery = this.db.PointsOfDeliveries.ToList(),
                Vehicles = this.db.Vehicles.ToList()
            };
            return this.View(locationsModel);
        }    
    }
}