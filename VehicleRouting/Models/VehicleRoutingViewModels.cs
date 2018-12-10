using System;
using System.Collections.Generic;

namespace VehicleRouting.Models
{
    public class ProductPackInputModel
    {
        public int ProductID { get; set; }
        public int PointOfDeliveryID { get; set; }
        public int Amount { get; set; }
    }

    public class LocationsViewModel
    {
        public IEnumerable<PointOfDelivery> PointsOfDelivery { get; set; }
        public IEnumerable<Vehicle> Vehicles { get; set; }
    }

    public class SolverViewModel
    {
        public LocationsViewModel LocationsViewModel { get; set; }
        public List<ProductPack> ProductPacks { get; set; }
        public List<int> VehiclesIDs { get; set; }
    }

    public class SolverReturnViewModel
    {
        public List<int> VehiclesIDs { get; set; }
        public List<int> ProductPacks { get; set; }
    }

    public class SolverResultViewModel
    {
        public SolverReturnViewModel SolverReturnViewModel { get; set; }
        public LocationsViewModel LocationsViewModel { get; set; }

        /// <summary>
        ///     Dictionary of keys - vehicle ids, values - sorted list of points they have to reach
        /// </summary>
        public Dictionary<int, List<ValueTuple<float, float>>> AlgorithmResult { get; set; }
    }
}