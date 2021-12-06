using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL
{
    public partial class BLObject : IBl
    {
        public double free;
        public double easy;
        public double medium;
        public double heavy;
        public double chargingRate;

        #region
       public static  Random rnd = new Random();
        public static IDAL.DO.IDal dl;
        List<DroneList> BODrones;
        public BLObject()
        {
            dl = new DalObject.DalObject();
            double[] arr = dl.AskElectrical();
            free = arr[0];
            easy = arr[1];
            medium = arr[2];
            heavy = arr[3];
            chargingRate = arr[4];
            List<IDAL.DO.Parcel> listParcels = dl.GetParcels().ToList();
            IEnumerable<DroneList> BODrones = from IDAL.DO.Drone item in dl.GetDrones().ToList()
                                              select new DroneList()
                                              {
                                                  Id = item.CodeDrone,
                                                  ModelDrone = item.ModelDrone,
                                                  Weight = (WeightCategories)item.MaxWeight,
                                                  DroneStatus = GetDroneStatus(item),
                                                  LocationNow=
                           };
        }
        public void UpdateSendingDroneToCharge(int id)
        {
            DroneList droneList = BODrones.Find(droneL => droneL.Id == id);
            if (droneList.Id == 0)
                throw new UpdateProblemException("This drone doesn't exist");
            if (droneList.DroneStatus == (DroneStatuses)0)
            {
                IDAL.DO.BaseStation closeStation = GetCloserBaseStation(droneList.LocationNow);
                double minDistance = GetDistance(new Location(closeStation.Longitude, closeStation.Latitude), droneList.LocationNow);
                if (droneList.Battery >= minDistance * free && closeStation.ChargeSlots > 0)
                {
                    //update drone
                    BODrones.Remove(droneList);
                    droneList.Battery -= minDistance * free;
                    droneList.LocationNow.Longitude = closeStation.Longitude;
                    droneList.LocationNow.Latitude = closeStation.Latitude;
                    droneList.DroneStatus = (DroneStatuses)1;
                    BODrones.Add(droneList);
                    //update station
                    dl.GetBaseStations().ToList().Remove(closeStation);
                    closeStation.ChargeSlots--;
                    dl.GetBaseStations().ToList().Add(closeStation);
                    //update drone Charge
                    DroneCharge droneCharge = new DroneCharge() { Battery = droneList.Battery, Id = droneList.Id };
                }
                else
                    throw new UpdateProblemException("It is impossible to send this drone to charging");
            }
            else
                throw new UpdateProblemException("The drone isn't free for charging");
        }
        public void UpdateReleasingDroneFromCharge(int id, double timeOfCharging)
        {
            DroneList droneList = BODrones.Find(droneL => droneL.Id == id);
            if (droneList.Id == 0)
                throw new UpdateProblemException("This drone doesn't exist");
            if (droneList.DroneStatus == (DroneStatuses)1)
            {
                //update drone
                BODrones.Remove(droneList);
                droneList.Battery = timeOfCharging * chargingRate;
                droneList.DroneStatus = (DroneStatuses)0;
                BODrones.Add(droneList);
                //update station
                IDAL.DO.BaseStation myStation = dl.GetBaseStations().ToList().Find(station => station.Longitude == droneList.LocationNow.Longitude && station.Latitude == droneList.LocationNow.Latitude);
                dl.GetBaseStations().ToList().Remove(myStation);
                myStation.ChargeSlots++;
                dl.GetBaseStations().ToList().Add(myStation);
            }
                
        }
        public void UpdateParcelToDrone(int id)
        {
            DroneList droneList = BODrones.Find(droneL => droneL.Id == id);
            if (droneList.Id == 0)
                throw new UpdateProblemException("This drone doesn't exist");
            if (droneList.DroneStatus == (DroneStatuses)0)
            {
                var groups = dl.GetParcels().ToList().GroupBy(parcel => parcel.Priority);
                List<IDAL.DO.Parcel> g0 = new List<IDAL.DO.Parcel>();
                List<IDAL.DO.Parcel> g1 = new List<IDAL.DO.Parcel>();
                List<IDAL.DO.Parcel> g2 = new List<IDAL.DO.Parcel>();
                foreach (IGrouping<Priorities, IDAL.DO.Parcel> group in groups)
                {
                    if (group.Key == (Priorities)0)
                        g0 = group.ToList();
                    else
                        if (group.Key == (Priorities)1)
                        g1 = group.ToList();
                    else
                        g2 = group.ToList();
                }
                var matchWeightParcels = from item in g2
                                         where ((int)item.Weight) <= ((int)droneList.Weight)
                                         select item;
                if (matchWeightParcels.ToList().Count != 0)
                {
                    IDAL.DO.Parcel closerParcel = new IDAL.DO.Parcel();
                    double minDistance = 10000000;
                    foreach (var item in matchWeightParcels)
                    {
                        IDAL.DO.Customer customer = dl.GetCustomer(item.SenderId);
                        double distance = GetDistance(new Location(customer.Longitude, customer.Latitude), droneList.LocationNow);
                        if (minDistance > distance)
                        {
                            minDistance = distance;
                            closerParcel = item;
                        }

                    }
                    if ()


                }


            }


        }
        public void UpdateParcelPickedUpByDrone(int droneID)
        {
            Drone drone = GetDrone(droneID);
            if (drone.DroneStatus==(DroneStatuses)2)
            {
                IDAL.DO.Parcel p = dl.GetParcels().ToList().Find(parcel => parcel.DroneId==droneID && parcel.PickedUp > DateTime.Now);
                //if the conditions match and the parcel was found
                if(p.DroneId==droneID)
                {
                    //update the drone and the parcel to be picked up by the drone 
                    Location senderLocation = new Location(dl.GetCustomer(p.SenderId).Longitude, dl.GetCustomer(p.SenderId).Latitude);
                    double distanceDroneToSender = GetDistance(drone.LocationNow, senderLocation);
                    DroneList boDrone = BODrones.Find(drone => drone.Id == droneID);
                    BODrones.Remove(boDrone);
                    boDrone.Battery -= free * distanceDroneToSender;
                    boDrone.LocationNow = senderLocation;
                    BODrones.Add(boDrone);
                    p.PickedUp = DateTime.Now;
                    dl.UpDateParcel(p);
                    return;
                }
            }
            throw new UpdateProblemException("This drone can't pick up the parcel");
        }
        public void UpdateDeliveredParcelByDrone(int idD)
        {

        }
        #endregion
    }
}
