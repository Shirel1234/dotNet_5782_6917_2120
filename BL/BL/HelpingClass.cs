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
        public static ParcelStatuses GetParcelStatus(IDAL.DO.Parcel parcel)
        {
            DateTime temp = new DateTime(0);
            if (parcel.Scheduled == temp)
                return (ParcelStatuses)0;
            if (parcel.PickedUp == temp)
                return (ParcelStatuses)1;
            if (parcel.Delivered == temp)
                return (ParcelStatuses)2;
            return (ParcelStatuses)3;
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
            IDAL.DO.Parcel p = dl.GetParcels().ToList().Find(parcel => parcel.DroneId == d.CodeDrone && GetParcelStatus(parcel) == (ParcelStatuses) 2 || GetParcelStatus(parcel) == (ParcelStatuses)1);
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
                Location locationTarget = new Location(dl.GetCustomer(p.TargetId).Longitude, dl.GetCustomer(p.TargetId).Latitude);
                double distanceDroneToTarget = GetDistance(newDroneList.LocationNow, locationTarget);
                double distanceTargetToCloserStation = GetDistance(locationTarget, new Location(GetCloserBaseStation(locationTarget).Longitude, GetCloserBaseStation(locationTarget).Latitude));
                double minCharge;
                //if the parcel has an easy weight
                minCharge = GetElectricityUseOfBattery(distanceDroneToTarget + distanceTargetToCloserStation, (WeightCategories) p.Weight);
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
                    int countStations = dl.GetBaseStations().ToList().Count();
                    IDAL.DO.BaseStation[] arrBaseStation = dl.GetBaseStations().ToArray();
                    IDAL.DO.BaseStation randomBaseStation = arrBaseStation[rnd.Next(0,countStations)];
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
                    IDAL.DO.BaseStation closerBaseStation = GetCloserBaseStation(newDroneList.LocationNow);
                    double distance = GetDistance(newDroneList.LocationNow, new Location(closerBaseStation.Longitude, closerBaseStation.Latitude));
                    newDroneList.Battery = rnd.Next((int)(distance * free),101);
                }
            }
            return newDroneList;

        }
        public static double GetElectricityUseOfBattery(double distance, WeightCategories weight)
        {
            //if the parcel has an easy weight
            if (weight == (WeightCategories)0)
                return distance * easy;
            else
                if (weight == (WeightCategories)1)
                //if the parcel has a medium weight
                    return distance * medium;
                else
                //the parcel has a heavy weight
                    return distance * heavy;
        }

        /// <summary>
        /// the function calculates the battery after the delivered by the distance that the drone did and 
        /// </summary>
        /// <param name="droneList"></param>
        /// <param name="parcel"></param>
        /// <returns></returns>
        public static double GetBatteryDeliveredParcel(DroneList droneList, IDAL.DO.Parcel parcel)
        {
            IDAL.DO.Customer targetCustomer = dl.GetCustomer(parcel.TargetId);
            double newBattery = droneList.Battery;
            double distance = GetDistance(droneList.LocationNow, new Location(targetCustomer.Longitude, targetCustomer.Latitude));
            newBattery -= GetElectricityUseOfBattery(distance, (WeightCategories)parcel.Weight);
            return newBattery;
        }
        public static IDAL.DO.Parcel GetParcelToDrone(List<IDAL.DO.Parcel> parcels, DroneList droneList)
        {
            IDAL.DO.Parcel chosenParcel = default;
            //list of parcels that matching the weight
            var matchWeightParcels = from item in parcels
                                     where ((int)item.Weight) <= ((int)droneList.Weight)
                                     select item;
            //find the closer parcel
            if (matchWeightParcels.ToList().Count != 0)
            {
                List<IDAL.DO.Parcel> closerParcels = (List<IDAL.DO.Parcel>)(from parcel in matchWeightParcels
                                                                            let customer = dl.GetCustomer(parcel.SenderId)
                                                                            let distance = GetDistance(new Location(customer.Longitude, customer.Latitude), droneList.LocationNow)
                                                                            orderby distance
                                                                            select parcel);
                //search the parcel that its battery will be enough
                foreach (var item in closerParcels)
                {
                    Location locationSender = new Location(dl.GetCustomer(item.SenderId).Longitude, dl.GetCustomer(item.SenderId).Latitude);
                    Location locationTarget = new Location(dl.GetCustomer(item.TargetId).Longitude, dl.GetCustomer(item.TargetId).Latitude);
                    double chargeBattery = GetDistance(droneList.LocationNow, locationSender) * free;
                    double distanceSenderToTarget = GetDistance(locationSender, locationTarget);
                    chargeBattery += GetElectricityUseOfBattery(distanceSenderToTarget, (WeightCategories)item.Weight);
                    IDAL.DO.BaseStation closerStation = GetCloserBaseStation(locationTarget);
                    double distanceTargetToCloserStation = GetDistance(locationTarget, new Location(closerStation.Longitude, closerStation.Latitude));
                    chargeBattery += distanceTargetToCloserStation * free;
                    if (chargeBattery <= droneList.Battery)
                    {
                        chosenParcel = item;
                        break;
                    }
                }
            }
            return chosenParcel;

        }

    }
}
