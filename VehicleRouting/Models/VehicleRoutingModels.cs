using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace VehicleRouting.Models
{
    public class Vehicle
    {
        public int ID { get; set; }
        public string Name { get; set; }

        [Range(1, int.MaxValue)]
        public int Capacity { get; set; }

        [Range(-180.00, 180.00)]
        [Display(Name = "Longitude of respawn")]
        public float SpawnPointX { get; set; }

        [Range(-90.00, 90.00)]
        [Display(Name = "Latitude of respawn")]
        public float SpawnPointY { get; set; }
    }

    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }

        [Range(1, int.MaxValue)]
        public int Weight { get; set; }

        public virtual ICollection<ProductPack> ProductPacks { get; set; }
    }

    public class PointOfDelivery
    {
        public int ID { get; set; }
        public string Name { get; set; }

        [Range(-180.00, 180.00)]
        [Display(Name = "Longitude")]
        public float CoordX { get; set; }

        [Range(-90.00, 90.00)]
        [Display(Name = "Latitude")]
        public float CoordY { get; set; }

        public virtual ICollection<ProductPack> ProductPacks { get; set; }
    }

    public class ProductPack
    {
        public int ID { get; set; }
        public int Amount { get; set; }

        public int ProductID { get; set; }

        public int PointOfDeliveryID { get; set; }

        [Required]
        public virtual Product Product { get; set; }

        [Required]
        public virtual PointOfDelivery PointOfDelivery { get; set; }
    }

    public class VehicleDbContext : DbContext
    {
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<PointOfDelivery> PointsOfDeliveries { get; set; }
        public DbSet<ProductPack> ProductPacks { get; set; }
    }

    public class ViewModel
    {
        public IEnumerable<PointOfDelivery> PointsOfDelivery { get; set; }
        public IEnumerable<Vehicle> Vehicles { get; set; }
    }
}