using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    namespace IDAL
    {
        namespace DO
        {
            public struct Parcel
            {
                public int CodeParcel { get; set; }
                public int SenderId { get; set; }
                public int TargetId { get; set; }
                public WeightCategories Weight { get; set; }
                public Priorities Priority { get; set; }
                public int DroneId { get; set; }
                public DateTime Requested { get; set; }
                public DateTime Scheduled { get; set; }
                public DateTime PickedUp { get; set; }
                public DateTime Delivered { get; set; }
                public override string ToString()
                {
                    return String.Format($"({CodeParcel}, /n{SenderId},/n {TargetID},/n{Weight},/n{Priority},/n{Drone},/n {Requested},/n{Scheduled},/n{PickedUp},/n{Delivered})");
                }



            }
        }
    }
}
