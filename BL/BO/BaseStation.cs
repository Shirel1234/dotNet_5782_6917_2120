using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class BaseStation
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public Location Location { get; set; }
        public int ChargeSlots { get; set; }
        public IEnumerable<DroneInCharge> ListDroneCharge { get; set; }
        public override string ToString()
        {
            return String.Format($"{Id}, {Name}, {Location}, {ChargeSlots}");
        }
    }
}
