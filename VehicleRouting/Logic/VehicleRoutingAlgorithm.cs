using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VehicleRouting.Models;

namespace VehicleRouting.Logic
{
    public class VehicleRoutingAlgorithm
    {
        private readonly VehicleDbContext db = new VehicleDbContext();

        private readonly SolverReturnViewModel solverReturnViewModel;

        private readonly string projectBin = $"{System.Web.HttpContext.Current.Server.MapPath("~")}bin";

        public VehicleRoutingAlgorithm(SolverReturnViewModel solverReturnViewModel)
        {
            this.solverReturnViewModel = solverReturnViewModel;
        }

        /// <summary>
        ///     Get best routes for all the vehicles that have some product packs inside them.
        /// </summary>
        /// <returns> Dictionary containing vehicle ids as keys and list of sorted points to reach as value. </returns>
        public Dictionary<int, List<ValueTuple<float, float>>> GetRoutes()
        {
            var dict = new Dictionary<int, List<ValueTuple<float, float>>>();

            Dictionary<int, List<ValueTuple<float, float>>> inputData = this.CreateInputData();

            foreach (int vehicle in inputData.Keys)
            {
                dict.Add(vehicle, this.RunAlgorithm(vehicle, inputData[vehicle]));
            }

            return dict;
        }

        /// <summary>
        ///     Converts Product pack ID to point of delivery location.
        /// </summary>
        /// <param name="ID"> Product pack ID. </param>
        /// <returns> Point of Delivery location of given product pack (by ID). </returns>
        private ValueTuple<float, float> GetPointOfDeliveryLocationFromProductPackID(int ID)
        {
            var pointOfDelivery = this.db.ProductPacks.First(v => v.ID == ID).PointOfDelivery;

            return new ValueTuple<float, float>(pointOfDelivery.CoordX, pointOfDelivery.CoordY);
        }

        /// <summary>
        ///     Creates input data.
        /// </summary>
        /// <returns> Dictionary containing vehicle ids as keys and list of points to reach (not sorted) as value. </returns>
        private Dictionary<int, List<ValueTuple<float, float>>> CreateInputData()
        {
            var dict = new Dictionary<int, List<int>>();

            for (int i = 0; i < this.solverReturnViewModel.ProductPacks.Count; i++)
            {
                int key = this.solverReturnViewModel.VehiclesIDs[i];
                int value = this.solverReturnViewModel.ProductPacks[i];
                if (dict.ContainsKey(key))
                    dict[key].Add(value);
                else
                    dict.Add(key, new List<int> {value});
            }

            var resultdict = new Dictionary<int, List<ValueTuple<float, float>>>();

            foreach (int key in dict.Keys)
            {
                var points = dict[key].Select(this.GetPointOfDeliveryLocationFromProductPackID).Distinct().ToList();
                resultdict.Add(key, points);
            }

            return resultdict;
        }

        /// <summary>
        ///     Executes the main algorithm on input data per vehicle.
        /// </summary>
        /// <param name="vehicle"> Vehicle that we are generating the route for. </param>
        /// <param name="pointsOfDelivery"> List of points to reach (not sorted) </param>
        /// <returns> List of sorted points to reach </returns>
        private List<ValueTuple<float, float>> RunAlgorithm(int vehicle, List<ValueTuple<float, float>> pointsOfDelivery)
        {
            this.CreateInputFile(vehicle, pointsOfDelivery);
            this.RunPythonAlgorithm(vehicle);
            return this.ParseOutputFile(vehicle);
        }

        private void CreateInputFile(int vehicleID, List<ValueTuple<float, float>> pointsOfDelivery)
        {
            string filePath = $"{this.projectBin}\\input{vehicleID}.txt";
            var veh = this.db.Vehicles.First(v => v.ID == vehicleID);
            var contents = new List<string> {$"{veh.SpawnPointX},{veh.SpawnPointY}"};
            contents.AddRange(pointsOfDelivery.Select(valueTuple => $"{valueTuple.Item1},{valueTuple.Item2}"));
            File.WriteAllLines(filePath, contents);
        }

        private void RunPythonAlgorithm(int vehicleID)
        {
            //TODO: implement some checks if the script was executed successfuly and created output files since for now we rely on the assumption that "THIS WORKS" :D

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo =
                new System.Diagnostics.ProcessStartInfo
                {
                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                    FileName = "cmd.exe",
                    Arguments = $"/C cd /D \"{this.projectBin}\" & python main.py {vehicleID}"
                };
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
        }

        private List<ValueTuple<float, float>> ParseOutputFile(int vehicleID)
        {
            var lines = File.ReadLines($"{this.projectBin}\\output{vehicleID}.txt");

            return lines.TakeWhile(line => line.Contains(',')).Select(line => line.Split(',')).Select(splits =>
                new ValueTuple<float, float>(float.Parse(splits[0]), float.Parse(splits[1]))).ToList();
        }
    }
}