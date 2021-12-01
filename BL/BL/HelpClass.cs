using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL.BO
{
   public class HelpClass
    { 
        public static double GetDistance(Location l1,Location l2)
        {
            return Math.Sqrt(Math.Pow(l1.Latitude - l2.Latitude, 2) + Math.Pow(l1.Longitude - l2.Longitude, 2));
        }
        public static ParcelStatuses GetParcelStatus(IDAL.DO.Parcel p)
        {
            if (DateTime.Compare(p.Delivered, DateTime.Now) <= 0)
                return (ParcelStatuses)3;
            if(DateTime.Compare(p.PickedUp, DateTime.Now) <= 0)
                return (ParcelStatuses)2;
            if (DateTime.Compare(p.Scheduled, DateTime.Now) <= 0)
                return (ParcelStatuses)1;
            return (ParcelStatuses)0;
        }
    }
}
