using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 namespace IDAL
    {
        namespace DO
        {
            
            public struct BaseStation
            {
                public int CodeStation { get; set; }
                public int NameStation { get; set; }
                public int ChargeSlots { get; set; }
                public double Longitude { get; set; }
                public double Latitude { get; set; }
                public override string ToString()
                {
                    return String.Format($"({CodeStation}, /n{NameStation},/n {ChargeSlots},/n{Longitude},/n{Latitude})");
                }
            }
        }
}
