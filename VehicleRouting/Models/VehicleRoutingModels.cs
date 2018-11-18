﻿using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace VehicleRouting.Models
{
    public class Vehicle
    {
        public int ID { get; set; }
        public string Name { get; set; }

        [Range(1, int.MaxValue)]
        public int Capacity { get; set; }

        // Spawn point
        public float SpawnPointX { get; set; }
        public float SpawnPointY { get; set; }
    }

    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }

        [Range(1, int.MaxValue)]
        public int Weight { get; set; }
    }

    public class PointOfDelivery
    {
        public int ID { get; set; }
        public string Name { get; set; }

        // Coordinates
        public float CoordX { get; set; }
        public float CoordY { get; set; }
    }

    public class VehicleDbContext : DbContext
    {
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<PointOfDelivery> PointsOfDeliveries { get; set; }
    }
}