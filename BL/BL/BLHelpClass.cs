using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BL
{
    public partial class BL
    {
        Random r = new Random();
        /// <summary>
        /// the function move from cordinates to distance in km by radius and math calculation
        /// </summary>
        /// <param name="l1"> first location</param>
        /// <param name="l2">second location </param>
        /// <returns> a distance between two locations</returns>
        private double GetDistance(Location l1, Location l2)
        {
            var R = 6371; // Radius of the earth in km
            var dLatitude = Getdeg2radius(l1.Latitude - l2.Latitude);  // deg2rad below
            var dLongitude = Getdeg2radius(l1.Longitude - l2.Longitude);
            var a =
              Math.Sin(dLatitude / 2) * Math.Sin(dLatitude / 2) +
              Math.Cos(Getdeg2radius(l1.Latitude)) * Math.Cos(Getdeg2radius(l2.Latitude)) *
              Math.Sin(dLongitude / 2) * Math.Sin(dLongitude / 2)
              ;
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c; // Distance in km
            return d;
        }
        private double Getdeg2radius(double d)
        {
            return d * (Math.PI / 180);
        }
        public ParcelStatuses GetDateTimeToStatus(DO.Parcel p)
        {
            if (p.Scheduled == null)
                return ParcelStatuses.requested;
            if (p.PickedUp == null)
                return ParcelStatuses.scheduled;
            if (p.Delivered == null)
                return ParcelStatuses.pickedUp;
            return ParcelStatuses.delivered;
        }
        /// <summary>
        /// the function moves on the base stations and find the closer station to the location 
        /// </summary>
        /// <param name="l"> location</param>
        /// <param name="listBaseStation"> list of base stations</param>
        /// <returns> the closer base station</returns>
        private DO.BaseStation GetCloserBaseStation(Location l)
        {
            double minDistance = 10000000;
            DO.BaseStation closeStation = new DO.BaseStation();
            foreach (var (item, distance) in from item in dal.GetStationsByCondition().ToList()
                                             let distance = GetDistance(new Location(item.Longitude, item.Latitude), l)
                                             where minDistance > distance && item.ChargeSlots>0
                                             select (item, distance))
            {
                minDistance = distance;
                closeStation = item;
            }

            return closeStation;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        private Location GetLocationNotPickedUp(DO.Parcel p)
        {
            DO.Customer customer = dal.GetCustomer(p.SenderId);
            DO.BaseStation closerBaseStation = GetCloserBaseStation(new Location(customer.Longitude, customer.Latitude));
            return new Location(closerBaseStation.Longitude, closerBaseStation.Latitude);
        }
        private ParcelStatuses GetParcelStatus(DO.Parcel parcel)
        {
            if (parcel.Scheduled == null)
                return ParcelStatuses.requested;
            if (parcel.PickedUp == null)
                return ParcelStatuses.scheduled;
            if (parcel.Delivered == null)
                return ParcelStatuses.pickedUp;
            return ParcelStatuses.delivered;
        }
        /// <summary>
        /// the function update the status of drone, location and battery
        /// </summary>
        /// <param name="d">drone</param>
        /// <returns> drone of list</returns>
        private DroneForList GetUpdatedDetailDrone(DO.Drone d)
        {
            try
            {
                //if the drone is in charge
                DroneForList newDrone = new DroneForList();
                DO.DroneCharge droneCharge = dal.GetDronesChargeByCondition().FirstOrDefault(dc => dc.DroneID == d.CodeDrone);
                if (droneCharge.DroneID != 0)
                {
                    newDrone.DroneStatus = DroneStatuses.maintenace;
                    DO.BaseStation baseStation = dal.GetStation(droneCharge.StationID);
                    newDrone.LocationNow = new Location(baseStation.Longitude, baseStation.Latitude);
                    TimeSpan dateTimeCharge = (TimeSpan)(DateTime.Now - droneCharge.BeginingCharge);
                    double battery = Convert.ToDouble(dateTimeCharge.Seconds * chargingRate);
                    if (battery > 100)
                        newDrone.Battery = 100;
                    else
                        newDrone.Battery = battery;
                    newDrone.ParcelInWay = 0;
                    return newDrone;
                }
                //DO.Parcel defaultP = default;
                //find the parcel of this drone that was scheduled or pickedUp
                DO.Parcel p = dal.GetParcelsByCondition().ToList().Find(parcel => parcel.DroneId == d.CodeDrone && (GetParcelStatus(parcel) == ParcelStatuses.pickedUp || GetParcelStatus(parcel) == ParcelStatuses.scheduled));
                //if this parcel was found
                if (p.CodeParcel != 0)
                {
                    newDrone.DroneStatus = DroneStatuses.sending;
                    newDrone.ParcelInWay = p.CodeParcel;
                    //if the parcel was scheduled but not pickedUp
                    if (GetDateTimeToStatus(p) == ParcelStatuses.scheduled)
                        newDrone.LocationNow = GetLocationNotPickedUp(p);
                    //the parcel was scheduled and pickedUp
                    else
                    {
                        DO.Customer c = dal.GetCustomer(p.SenderId);
                        newDrone.LocationNow = new Location(c.Longitude, c.Latitude);
                    }
                    //calculate the battery by the minimum distance that the drone needs to pass
                    List<DO.Customer> customerList = dal.GetCustomersByCondition().ToList();
                    DO.Customer cust = customerList.Find(c => c.IdCustomer == p.TargetId);
                    Location locationTarget = new Location(cust.Longitude, cust.Latitude);
                    double distanceDroneToTarget = GetDistance(newDrone.LocationNow, locationTarget);
                    DO.BaseStation b = GetCloserBaseStation(locationTarget);
                    Location l = new Location(b.Longitude, b.Latitude);
                    double distanceTargetToCloserStation = GetDistance(locationTarget, l);
                    double minCharge;
                    //if the parcel has an easy weight
                    minCharge = GetElectricityUseOfBattery(distanceDroneToTarget + distanceTargetToCloserStation, (WeightCategories)p.Weight);
                    if (minCharge > 100)
                        newDrone.Battery = 100;
                    else
                    {
                        int intMinCharge = Convert.ToInt32(minCharge);
                        newDrone.Battery = r.NextDouble()+ r.Next(intMinCharge, 101);
                    }
                }
                //the drone doesn't do sending
                else
                {
                    int num = r.Next(0, 2);
                    newDrone.DroneStatus = (DroneStatuses)num;
                    newDrone.ParcelInWay = 0;
                    //the drone is in maintenace
                    if (newDrone.DroneStatus == DroneStatuses.maintenace)
                    {
                        newDrone.Battery = r.Next(0, 21);
                        DO.BaseStation baseStation;
                        //random a base station
                        if (d.CodeDrone % 2 != 0)
                        {
                            baseStation = dal.GetStation(1);
                            newDrone.LocationNow = new Location(baseStation.Longitude, baseStation.Latitude);
                        }
                        else
                        {
                            baseStation = dal.GetStation(2);
                            newDrone.LocationNow = new Location(baseStation.Longitude, baseStation.Latitude);
                        }
                        //DO.BaseStation[] arrBaseStation = dal.GetStationsByCondition().ToArray();
                        //int randomNumOfStationForCharge = r.Next(0, 2);
                        //DO.BaseStation randomBaseStation = arrBaseStation[randomNumOfStationForCharge];
                        ////check if the station which was randomed has available charge slots. If not, the other station must have.
                        //if (randomBaseStation.ChargeSlots <= 0)
                        //    randomBaseStation = arrBaseStation[1 - randomNumOfStationForCharge];
                        //newDrone.LocationNow = new Location(randomBaseStation.Longitude, randomBaseStation.Latitude);
                        baseStation.ChargeSlots--;
                        dal.AddDroneCharge(d.CodeDrone, baseStation.CodeStation, DateTime.Now);
                        dal.UpDateBaseStation(baseStation);
                    }
                    //the drone is free
                    else
                    {
                        //random a customer that got at least one parcel
                        var listCustomerGotParcel = from parcel in dal.GetParcelsByCondition().ToList()
                                                    where parcel.Delivered <= DateTime.Now
                                                    select dal.GetCustomersByCondition(c => c.IdCustomer == parcel.TargetId).ToList();
                        int count = listCustomerGotParcel.Count();
                        DO.Customer[] arrCustomerGotParcel = new DO.Customer[count];
                        DO.Customer randomCustomer = arrCustomerGotParcel[r.Next(count)];
                        newDrone.LocationNow = new Location(randomCustomer.Longitude, randomCustomer.Latitude);
                        //random battery between minimum of arriving to base station for charge
                        DO.BaseStation closerBaseStation = GetCloserBaseStation(newDrone.LocationNow);
                        closerBaseStation.ChargeSlots--;
                        dal.UpDateBaseStation(closerBaseStation);
                        double distance = GetDistance(newDrone.LocationNow, new Location(closerBaseStation.Longitude, closerBaseStation.Latitude));
                        if ((distance * free) > 100)
                            newDrone.Battery = 100;
                        else
                            newDrone.Battery = r.Next((int)(distance * free), 101);
                    }
                }
                return newDrone;
            }
            catch (Exception ex)
            {
                throw new GetDetailsProblemException(ex.Message);
            }
            
        }
        private double GetElectricityUseOfBattery(double distance, WeightCategories weight)
        {
            //if the parcel has an easy weight
            if (weight == WeightCategories.easy)
                return distance * easy;
            else
                if (weight == WeightCategories.medium)
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
        public double GetBatteryDeliveredParcel(DroneForList droneList, DO.Parcel parcel)
        {
            DO.Customer targetCustomer = dal.GetCustomer(parcel.TargetId);
            double newBattery = droneList.Battery;
            double distance = GetDistance(droneList.LocationNow, new Location(targetCustomer.Longitude, targetCustomer.Latitude));
            newBattery -= GetElectricityUseOfBattery(distance, (WeightCategories)parcel.Weight);
            return newBattery;
        }
        /// <summary>
        /// the function search
        /// </summary>
        /// <param name="parcels">list posibble of parcels for scheduling</param>
        /// <param name="droneList">drone for match it parcel </param>
        /// <returns></returns>
        private DO.Parcel GetParcelToDrone(List<DO.Parcel> parcels, DroneForList droneList)
        {
            DO.Parcel chosenParcel = default;
            //list of parcels that matching the weight
            IEnumerable<DO.Parcel> matchWeightParcels = from item in parcels
                                     where ((int)item.Weight) <= ((int)droneList.Weight)
                                     select item;
            //find the closer parcel
            if (matchWeightParcels.ToArray()[0].CodeParcel!=0)
            {
                IEnumerable<DO.Parcel> closerParcels = (from parcel in matchWeightParcels.ToList()
                                                                  let customer = dal.GetCustomer(parcel.SenderId)
                                                                  let distance = GetDistance(new Location(customer.Longitude, customer.Latitude), droneList.LocationNow)
                                                                  orderby distance
                                                                  select parcel);
                //search the parcel that its battery will be enough
                foreach (var item in closerParcels)
                {
                    Location locationSender = new Location(dal.GetCustomer(item.SenderId).Longitude, dal.GetCustomer(item.SenderId).Latitude);
                    Location locationTarget = new Location(dal.GetCustomer(item.TargetId).Longitude, dal.GetCustomer(item.TargetId).Latitude);
                    double chargeBattery = GetDistance(droneList.LocationNow, locationSender) * free;
                    double distanceSenderToTarget = GetDistance(locationSender, locationTarget);
                    chargeBattery += GetElectricityUseOfBattery(distanceSenderToTarget, (WeightCategories)item.Weight);
                    DO.BaseStation closerStation = GetCloserBaseStation(locationTarget);
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

