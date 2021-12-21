using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
   public class ParcelForList
    {
        public int Id { get; set; }
        public string NameSender { get; set; }
        public string NameTarget { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public ParcelStatuses Status { get; set; }
        public override string ToString()
        {
            return String.Format($"{Id}, {NameSender}, {Weight}, {NameTarget}, {Priority}, {Status}");
        }

    }
}
