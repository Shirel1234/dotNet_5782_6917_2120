using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
      public class Parcel
        {
            public int CodeParcel { get; set; }
            public int SenderCustomerId { get; set; }
            public int TargetCustomerId { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public int DroneId { get; set; }
            public DateTime Requested { get; set; }
            public DateTime Scheduled { get; set; }
            public DateTime PickedUp { get; set; }
            public DateTime Delivered { get; set; }
            public override string ToString()
            {
                return String.Format($"({CodeParcel}, /n{SenderCustomerId},/n {TargetCustomerId},/n{Weight},/n{DroneId},/n {Requested},/n{Scheduled},/n{PickedUp},/n{Delivered})");
            }
        }
    }
}
