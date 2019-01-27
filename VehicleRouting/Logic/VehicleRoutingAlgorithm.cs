﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using VehicleRouting.Models;

namespace VehicleRouting.Logic
{
    public class VehicleRoutingAlgorithm
    {
        private readonly VehicleDbContext db = new VehicleDbContext();

        private readonly SolverReturnViewModel solverReturnViewModel;

        private readonly string projectBin = $"{System.Web.HttpContext.Current.Server.MapPath("~")}bin";

        private Dictionary<int, List<ValueTuple<float, float>>> inputData;

        private readonly Dictionary<int, List<ValueTuple<float, float>>> detailedOutput = new Dictionary<int, List<ValueTuple<float, float>>>();

        private Dictionary<int, int> outputSeparators = new Dictionary<int, int>();

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

            this.inputData = this.CreateInputData();

            foreach (int vehicle in this.inputData.Keys)
            {
                this.outputSeparators[vehicle] = this.inputData[vehicle].Count + 1;
                dict.Add(vehicle, this.RunAlgorithm(vehicle, this.inputData[vehicle]));
                this.detailedOutput.Add(vehicle, this.ParseDetailedOutput(vehicle));
            }

            return dict;
        }


        /// <summary>
        ///     Get best routes for all the vehicles that have some product packs inside them.
        /// </summary>
        /// <returns> Dictionary containing vehicle ids as keys and list of sorted points to reach as value (including intermediate points). </returns>
        public Dictionary<int, List<ValueTuple<float, float>>> GetDetailedRoutes()
        {
            if (this.detailedOutput == null)
                this.GetRoutes();
            return this.detailedOutput;
        }

        /// <summary>
        ///     Get a dictionary containing time of computing and distance for each vehicle after algorithm execution
        /// </summary>
        /// <returns> Dictionary containing vehicle ids as keys and time and distance as values. </returns>
        public Dictionary<int, ValueTuple<float, float>> GetTimeAndDistance()
        {
            var dict = new Dictionary<int, ValueTuple<float, float>>();

            foreach (var key in this.inputData.Keys)
            {
                var lines = File.ReadLines($"{this.projectBin}\\output{key}.txt").ToList();
                dict.Add(key, new ValueTuple<float, float>(float.Parse(lines[this.outputSeparators[key] + 1]), float.Parse(lines[this.outputSeparators[key] + 2])));
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
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo =
                new System.Diagnostics.ProcessStartInfo
                {
                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                    FileName = "cmd.exe",
                    Arguments = $"/C cd /D \"{this.projectBin}\" & python main.py {vehicleID} {this.solverReturnViewModel.MetricType}",
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                };
            process.StartInfo = startInfo;
            process.Start();
            if (!process.WaitForExit((int)TimeSpan.FromMinutes(2).TotalMilliseconds))
                throw new TimeoutException("Python algorithm timeout (possibly too much data, use CLI version)");
            string stderr = process.StandardError.ReadToEnd();
            if ( stderr!= string.Empty || process.ExitCode != 0)
                throw new CommunicationException($"Python scripts failed! {stderr}");
        }

        private List<ValueTuple<float, float>> ParseDetailedOutput(int vehicleID)
        {
            var lines = File.ReadLines($"{this.projectBin}\\output{vehicleID}.txt").Skip(this.outputSeparators[vehicleID] + 3);
            return this.ParseLines(lines);
        }

        private List<ValueTuple<float, float>> ParseOutputFile(int vehicleID)
        {
            var lines = File.ReadLines($"{this.projectBin}\\output{vehicleID}.txt");
            return this.ParseLines(lines);
        }

        private List<ValueTuple<float, float>> ParseLines(IEnumerable<string> lines)
        {
            return lines.TakeWhile(line => line.Contains(',')).Select(line => line.Split(',')).Select(splits =>
                new ValueTuple<float, float>(float.Parse(splits[0]), float.Parse(splits[1]))).ToList();
        }
    }
}