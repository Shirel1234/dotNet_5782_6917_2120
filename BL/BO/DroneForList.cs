using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
  public  class DroneForList
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
            return String.Format($"{Id}, {ModelDrone}, {Weight}, {DroneStatus}, {Battery}, {LocationNow}, {ParcelInWay}");
        }
    }
}
