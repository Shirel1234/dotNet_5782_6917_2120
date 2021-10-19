using System;
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
                public int ModelDrone { get; set; }
                public int MaxWeight { get; set; }
                public int Status { get; set; }
                public int Battery { get; set; }
            }
        }
    }
    
}
