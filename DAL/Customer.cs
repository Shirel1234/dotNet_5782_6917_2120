using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 namespace IDAL
  {
    namespace DO
    {
            public struct Customer
            {
                public int IdCustomer { get; set; }
                public string NameCustomer { get; set; }
                public string Phone { get; set; }
                public double Longitude { get; set; }
                public double Latitude { get; set; }
                public override string ToString()
                {
                    return String.Format($"({IdCustomer}, /n{NameCustomer},/n {Phone},/n{Longitude},/n{Latitude})");
                }
            }
        }
    }


