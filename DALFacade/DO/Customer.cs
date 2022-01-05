using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public struct Customer
    {
        public int IdCustomer { get; set; }
        public string NameCustomer { get; set; }
        public string Phone { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public bool IsWorker { get; set; }
        public override string ToString()
        {
            return String.Format($"(IdCustomer: {IdCustomer}, NameCustomer: {NameCustomer}, Phone: {Phone}, Longitude:{Longitude}, Latitude:{Latitude} Is Worker: {IsWorker})");
        }
    }
}



