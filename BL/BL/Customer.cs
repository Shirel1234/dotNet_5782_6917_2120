using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class Customer
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public Location Location { get; set; }
            public IEnumerable<ParcelCustomer> SendParcels { get; set; }
            public IEnumerable<ParcelCustomer> Taretparcels { get; set; }
            public override string ToString()
            {
                return String.Format($"({Id}, /n{Name}, /n{Phone}, /n{Location})");
            }
        }
    }
}
