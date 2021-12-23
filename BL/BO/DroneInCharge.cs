using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class DroneInCharge
    {
        public int Id { get; set; }
        public double Battery { get; set; }
        public DateTime? DateTimeBegining { get; set; }
        public override string ToString()
        {
            return String.Format($"Id: {Id}, Battery: {Battery}, DateTime Begining:{DateTimeBegining} ");
        }
    }
}

