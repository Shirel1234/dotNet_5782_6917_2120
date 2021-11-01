﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
namespace IDAL
    {
        namespace DO
        {
            public struct Drone
            {
                public int CodeDrone { get; set; }
                public string ModelDrone { get; set; }
                public WeightCategories MaxWeight { get; set; }
                public DroneStatuses Status { get; set; }
                public double Battery { get; set; }
                public override string ToString()
                {
                    return String.Format($"({CodeDrone}, /n{ModelDrone},/n {MaxWeight},/n{Status},/n{Battery})");
                }
            }
        }
    }
    
}