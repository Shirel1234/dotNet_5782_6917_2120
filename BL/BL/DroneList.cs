using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
  public  class DroneList
    {
        public int Id { set; get; }
        public string ModelDrone { set; get; }
        public WeightCategories Weight { set; get; }
        public DroneStatuses DroneStatus { set; get; }
        public double Battery { set; get; }
        public Location LocationNow { set; get; }
        public int ParcelInWay { set; get; }
        public override string ToString()
        {
            return String.Format($"({Id}, \n{ModelDrone})\n{Weight})\n{DroneStatus})\n{Battery}\n{LocationNow}\n{ParcelInWay}");
        }
    }
}
