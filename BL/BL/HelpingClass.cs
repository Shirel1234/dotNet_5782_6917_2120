using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL
{
    public partial class BLObject
    {
        public static double GetDistance(Location l1, Location l2)
        {
            return Math.Sqrt(Math.Pow(l1.Latitude - l2.Latitude, 2) + Math.Pow(l1.Longitude - l2.Longitude, 2));
        }
        public static ParcelStatuses GetDateTimeToStatus(IDAL.DO.Parcel p)
        {
            if (DateTime.Compare(p.Delivered, DateTime.Now) <= 0)
                return (ParcelStatuses)3;
            if (DateTime.Compare(p.PickedUp, DateTime.Now) <= 0)
                return (ParcelStatuses)2;
            if (DateTime.Compare(p.Scheduled, DateTime.Now) <= 0)
                return (ParcelStatuses)1;
            return (ParcelStatuses)0;
        }
        /// <summary>
        /// the function moves on the base stations and find the closer station to the location 
        /// </summary>
        /// <param name="l"> location</param>
        /// <param name="listBaseStation"> list of base stations</param>
        /// <returns> the closer base station</returns>
        public static IDAL.DO.BaseStation GetCloserBaseStation(Location l)
        {
            double minDistance = 10000000;
            IDAL.DO.BaseStation closeStation = new IDAL.DO.BaseStation();
            foreach (var item in dl.GetBaseStations().ToList())
            {
                double distance = GetDistance(new Location(item.Longitude, item.Latitude), l);
                if (minDistance > distance)
                {
                    minDistance = distance;
                    closeStation = item;
                }
            }
            return closeStation;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        public static Location GetLocationNotPickedUp(IDAL.DO.Parcel p)
        {
            IDAL.DO.Customer customer = dl.GetCustomer(p.SenderId);
            IDAL.DO.BaseStation closerBaseStation = GetCloserBaseStation(new Location(customer.Longitude, customer.Latitude));
            return new Location(closerBaseStation.Longitude, closerBaseStation.Latitude);
        }
        public static DroneList GetUpdatedDetailDrone(IDAL.DO.Drone d)
        {
            IDAL.DO.Parcel defaultP = default;
            DroneList newDroneList = new DroneList();
            IDAL.DO.Parcel p = dl.GetParcels().ToList().Find(parcel => parcel.DroneId == d.CodeDrone && GetParcelStatus(parcel) == (ParcelStatuses)2 || GetParcelStatus(parcel) == (ParcelStatuses)1);
            if (p.CodeParcel != defaultP.CodeParcel)
            {
                newDroneList.DroneStatus = (DroneStatuses)2;
                if (GetDateTimeToStatus(p) == (ParcelStatuses)1)
                    newDroneList.LocationNow = GetLocationNotPickedUp(p);
                else
                    newDroneList.LocationNow = new Location(dl.GetCustomer(p.SenderId).Longitude, dl.GetCustomer(p.SenderId).Latitude);
            }
            else
                newDroneList.DroneStatus = (DroneStatuses)rnd.Next(0, 2);

        }
    }
}
