﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    public class Drone
    {
        public int Id { get; set; }
        public string ModelDrone { get; set; }
        public WeightCategories MaxWeight { get; set; }
        public DroneStatuses DroneStatus { get; set; }
        public double Battery { get; set; }
        public ParcelInWay ParcelInWay { get; set; }
        public Location LocationNow { get; set; }
        public override string ToString()
        {
            return String.Format($"Id: {Id}, Model Drone:{ModelDrone}, Max Weight: {MaxWeight}, Drone Status: {DroneStatus}, Battery: {Battery}%, Parcel In Way: {ParcelInWay},  LocationNow: {LocationNow}");
        }
    }
}

