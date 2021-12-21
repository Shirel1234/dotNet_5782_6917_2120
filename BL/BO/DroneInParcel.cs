using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
   public class DroneInParcel
    {
      public int Id { set; get; }
      public double Battery { set; get; }
      public Location LocationNow { set; get; }
        public override string ToString()
        {
            return String.Format($"{Id}, {Battery}, {LocationNow}");
        }
    }
}
