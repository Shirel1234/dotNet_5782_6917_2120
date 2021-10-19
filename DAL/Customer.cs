using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    namespace IDAL
    {
        namespace Do
        {
            public struct Customer
            {
                public int IdCustomer { get; set; }
                public string NameCustomer { get; set; }
                public string Phone { get; set; }
                public double Longitude { get; set; }
                public double Latitude { get; set; }
            }
        }
    }
}

