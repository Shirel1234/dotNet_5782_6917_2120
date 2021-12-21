using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class ParcelInCustomer
    {
        public int Id { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public ParcelStatuses Status { get; set; }
        public CustomerInParcel SecondSideCustomer { get; set; }
        public override string ToString()
        {
            return string.Format($"{Id}, {Weight}, {Priority}, {Status}, {SecondSideCustomer}");
        }
    }
}
