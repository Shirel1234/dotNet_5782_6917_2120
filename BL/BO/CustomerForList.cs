using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
   public class CustomerForList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int CountDeliveredParcels { get; set; }
        public int CountNotDeliveredParcels { get; set; }
        public int CountAcceptedParcelsByCustomer { get; set; }
        public int CountParcelsInWay { get; set; }
        public override string ToString()
        {
            return String.Format($"{Id}, {Name}, {Phone}, {CountDeliveredParcels}, {CountNotDeliveredParcels}, {CountAcceptedParcelsByCustomer}, {CountParcelsInWay}");
        }
    }
}
