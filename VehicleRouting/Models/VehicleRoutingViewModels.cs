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
}