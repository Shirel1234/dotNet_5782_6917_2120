using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    public class Parcel
    {
        public int CodeParcel { get; set; }
        public CustomerInParcel SenderCustomer { get; set; }
        public CustomerInParcel TargetCustomer { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public DroneInParcel DroneInParcel { get; set; }
        public DateTime? Requested { get; set; }//זמן יצירת החבילה
        public DateTime? Scheduled { get; set; }//זמן שיוך
        public DateTime? PickedUp { get; set; }//זמן איסוף
        public DateTime? Delivered { get; set; }//זמן אספקה
        public override string ToString()
        {
            return String.Format($"Code Parcel: {CodeParcel}, Sender Customer: {SenderCustomer}, TargetCustomer: {TargetCustomer}, Weight: {Weight}, Drone In Parcel: {DroneInParcel}, Requested: {Requested}, Scheduled: {Scheduled}, PickedUp: {PickedUp}, Delivered: {Delivered}");
        }
    }
}

