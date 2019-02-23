using System;
using System.Linq;
using System.ServiceModel;
using System.Web.Mvc;
using VehicleRouting.Logic;
using VehicleRouting.Models;

namespace VehicleRouting.Controllers
{
    public class SolverController : Controller
    {
        private readonly VehicleDbContext db = new VehicleDbContext();

        [AllowAnonymous]
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
        [AllowAnonymous]
        public ActionResult Index(SolverReturnViewModel solverReturnViewModel)
        {
            if (!this.ModelState.IsValid) return this.Index();

            LocationsViewModel locationsModel = new LocationsViewModel
            {
                PointsOfDelivery = this.db.PointsOfDeliveries.ToList(),
                Vehicles = this.db.Vehicles.ToList()
            };

            var algorithm = new VehicleRoutingAlgorithm(solverReturnViewModel);
            var solverResultViewModel = new SolverResultViewModel();

            try
            {
                solverResultViewModel.LocationsViewModel = locationsModel;
                solverResultViewModel.AlgorithmResult = algorithm.GetRoutes();
                solverResultViewModel.AlgorithmDetailedResult = algorithm.GetDetailedRoutes();
                solverResultViewModel.TimeAndDistance = algorithm.GetTimeAndDistance();

            }
            catch (TimeoutException ex)
            {
                this.ViewBag.Error =
                    "Algorithm execution timed out. Possibly too much input data or OSRM server not responding.";
                return this.View("Error");
            }
            catch (CommunicationException ex)
            {
                this.ViewBag.Error = "Algorithm execution failed!";
                return this.View("Error");
            }

            return this.View("Result", solverResultViewModel);

        }
    }
}