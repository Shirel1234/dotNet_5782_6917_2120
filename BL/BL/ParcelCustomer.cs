using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace IBL.BO
{
    public class ParcelCustomer
    {
        public int Id { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public ParcelStatuses Status { get; set; }
        public int CustomerId { get; set; }
        public override string ToString()
        {
            return String.Format($"{Id}, {Weight}, {Priority},{Status}, {CustomerId}, ");
        }
    }
}