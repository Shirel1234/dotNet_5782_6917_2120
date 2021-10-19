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
            public struct Parcel
            {
                public int CodeParcel { get; set; }
                public int SenderId { get; set; }
                public int TargetID { get; set; }
                public int Weight { get; set; }
                public double Priority { get; set; }
                public int Drone { get; set; }
                public dateTime Requested { get; set; }
                public dateTime Scheduled { get; set; }
                public dateTime PickedUp { get; set; }
                public dateTime Delivered { get; set; }




            }
        }
    }
}
