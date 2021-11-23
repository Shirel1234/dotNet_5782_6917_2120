using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
       public class Location
        {
                public double Longitude{ get; set; }
                public double Latitude{ get; set; }
            public Location(double longitude, double latitude)
            {
                this.Longitude = longitude;
                this.Latitude = latitude;
            }
        }
    }
}
