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
            LocationsViewModel locationsModel = new LocationsViewModel
            {
                PointsOfDelivery = this.db.PointsOfDeliveries.ToList(),
                Vehicles = this.db.Vehicles.ToList()
            };

            SolverViewModel solverViewModel = new SolverViewModel
            {
                LocationsViewModel = locationsModel,
                ProductPacks = this.db.ProductPacks.ToList()
            };

            this.ViewBag.Vehicles =
                this.db.Vehicles.Select(v => new SelectListItem { Text = v.Name, Value = v.ID.ToString() }).ToList();

            return this.View(solverViewModel);
        }


        // POST: Solver/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(SolverReturnViewModel solverReturnViewModel)
        {
            LocationsViewModel locationsModel = new LocationsViewModel
            {
                PointsOfDelivery = this.db.PointsOfDeliveries.ToList(),
                Vehicles = this.db.Vehicles.ToList()
            };

            return this.View("Result", locationsModel);
        }
    }
}