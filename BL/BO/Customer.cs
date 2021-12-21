﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
     public  class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public Location Location { get; set; }
        public IEnumerable<ParcelInCustomer> SendParcels { get; set; }
        public IEnumerable<ParcelInCustomer> TargetParcels { get; set; }
        public override string ToString()
        {
            return String.Format($"{Id}, {Name}, {Phone}, {Location},{SendParcels},{TargetParcels}");
        }
    }
}
