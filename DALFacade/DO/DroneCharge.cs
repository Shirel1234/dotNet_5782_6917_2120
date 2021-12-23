using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DO
{
    public struct DroneCharge
    {
        public int DroneID { get; set; }
        public int StationID { get; set; }
        public DateTime? BeginingCharge { get; set; }
        public override string ToString()
        {
            return String.Format($"(Drone ID: {DroneID}, Station ID: {StationID}, Begining Charge: {BeginingCharge})");
        }
    }
}


