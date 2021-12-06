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
        public static int GetParcelStatus(IDAL.DO.Parcel parcel)
        {
            DateTime temp = new DateTime(0);
            if (parcel.Scheduled == temp)
                return 0;
            if (parcel.PickedUp == temp)
                return 1;
            if (parcel.Delivered == temp)
                return 2;
            return 3;
        }
        /// <summary>
        /// the function update the status of drone, location and battery
        /// </summary>
        /// <param name="d">drone</param>
        /// <returns> drone of list</returns>
        public static DroneList GetUpdatedDetailDrone(IDAL.DO.Drone d)
        {
            IDAL.DO.Parcel defaultP = default;
            DroneList newDroneList = new DroneList();
            //find the parcel of this drone that was scheduled or pickedUp
            IDAL.DO.Parcel p = dl.GetParcels().ToList().Find(parcel => parcel.DroneId == d.CodeDrone && GetParcelStatus(parcel) ==2 || GetParcelStatus(parcel) == 1);
            //if this parcel was found
            if (p.CodeParcel != defaultP.CodeParcel)
            {
                newDroneList.DroneStatus = (DroneStatuses)2;
                //if the parcel was scheduled but not pickedUp
                if (GetDateTimeToStatus(p) == (ParcelStatuses)1)
                    newDroneList.LocationNow = GetLocationNotPickedUp(p);
                //the parcel was scheduled and pickedUp
                else
                    newDroneList.LocationNow = new Location(dl.GetCustomer(p.SenderId).Longitude, dl.GetCustomer(p.SenderId).Latitude);
                //calculate the battry by the minimum distance that the drone needs to pass
                Location  locationTarget = new Location (dl.GetCustomer(p.TargetId).Longitude, dl.GetCustomer(p.TargetId).Latitude);
                double distanceDroneToTarget = GetDistance(newDroneList.LocationNow, locationTarget);
                double distanceTargetToCloserStation = GetDistance(locationTarget, new Location(GetCloserBaseStation(locationTarget).Longitude, GetCloserBaseStation(locationTarget).Latitude));
                double minCharge;
                //if the parcel has an easy weight
                if (p.Weight == (IDAL.DO.WeightCategories)0)
                    minCharge = (distanceDroneToTarget + distanceTargetToCloserStation) * easy;
                else
                //if the parcel has a medium weight
                   if (p.Weight == (IDAL.DO.WeightCategories)1)
                      minCharge = (distanceDroneToTarget + distanceTargetToCloserStation) * medium;
                   else
                    //the parcel has a heavy weight
                    minCharge = (distanceDroneToTarget + distanceTargetToCloserStation) * heavy;
                newDroneList.Battery = rnd.Next((int)minCharge, 101);
            }
            //the drone doesn't do sending
            else
            {
                newDroneList.DroneStatus = (DroneStatuses)rnd.Next(0, 2);
                //the drone is in maintenace
                if (newDroneList.DroneStatus == (DroneStatuses)1)
                {
                    newDroneList.Battery = rnd.Next(0, 21);
                    //random a base station
                    int countStations =dl.GetBaseStations().ToList().Count();
                    IDAL.DO.BaseStation[] arrBaseStation = dl.GetBaseStations().ToArray();
                    IDAL.DO.BaseStation randomBaseStation = arrBaseStation[rnd.Next(countStations)];
                    newDroneList.LocationNow = new Location(randomBaseStation.Longitude, randomBaseStation.Latitude);
                }
                //the drone is free
                else
                {
                    //random a customer that got at least one parcel
                    var listCustomerGotParcel = from parcel in dl.GetParcels().ToList()
                                                where parcel.Delivered <= DateTime.Now
                                                select dl.GetCustomer(parcel.TargetId);
                    int count = listCustomerGotParcel.Count();
                    IDAL.DO.Customer[] arrCustomerGotParcel = new IDAL.DO.Customer[count];
                    IDAL.DO.Customer randomCustomer = arrCustomerGotParcel[rnd.Next(count)];
                    newDroneList.LocationNow = new Location(randomCustomer.Longitude, randomCustomer.Latitude);
                    //random battery between minimum of arriving to base station for charge
                    IDAL.DO.BaseStation closerBaseStation=GetCloserBaseStation(newDroneList.LocationNow);
                    double distance = GetDistance(newDroneList.LocationNow, new Location(closerBaseStation.Longitude, closerBaseStation.Latitude));
                    newDroneList.Battery = distance * free;
                }
            }
            return newDroneList;

        }
        public static double ElectricityUseOfBattery(double distance,WeightCategories weight)
        {
            if (weight == (WeightCategories)0)
                return distance * easy;
            else
                if (weight == (WeightCategories)1)
                return distance * medium;
            else
                return distance * heavy;
        }
    }
        /// <summary>
        /// the function calculates the battery after the delivered by the distance that the drone did and 
        /// </summary>
        /// <param name="droneList"></param>
        /// <param name="parcel"></param>
        /// <returns></returns>
        public static double GetBatteryDeliveredParccel(DroneList droneList, IDAL.DO.Parcel parcel)
        {
            IDAL.DO.Customer targetCustomer = dl.GetCustomer(parcel.TargetId);
            double newBattery = droneList.Battery;
            double distance = GetDistance(droneList.LocationNow, new Location(targetCustomer.Longitude, targetCustomer.Latitude));
            if (parcel.Weight == (IDAL.DO.WeightCategories)0)
                newBattery = newBattery- (distance*easy);
            else
                  //if the parcel has a medium weight
                  if (parcel.Weight == (IDAL.DO.WeightCategories)1)
                newBattery = newBattery - (distance * medium);
            else
                //the parcel has a heavy weight
                newBattery = newBattery - (distance * heavy);
            return newBattery;
        }
}
