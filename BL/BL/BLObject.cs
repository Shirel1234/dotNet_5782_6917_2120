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
        public static double free;
        public static double easy;
        public static double medium;
        public static double heavy;
        public static double chargingRate;

        #region
        public static Random rnd = new Random();
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
                                                  DroneStatus = GetUpdatedDetailDrone(item).DroneStatus,
                                                  LocationNow = GetUpdatedDetailDrone(item).LocationNow,
                                                  Battery = GetUpdatedDetailDrone(item).Battery,
                                                  ParcelInWay = 0
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
                    closeStation.ChargeSlots--;
                    dl.UpDateBaseStation(closeStation);
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
                myStation.ChargeSlots++;
                dl.UpDateBaseStation(myStation);
            }

        }
        public void UpdateParcelToDrone(int id)
        {
            IDAL.DO.Parcel chosenParcel=default;
            //bring the details of this drone
            DroneList droneList = BODrones.Find(droneL => droneL.Id == id);
            if (droneList.Id == 0)
                throw new UpdateProblemException("This drone doesn't exist");
            //check that the drone is free
            if (droneList.DroneStatus == (DroneStatuses)0)
            {
                //create three groops by the type of priority
                var groups = dl.GetParcels().ToList().GroupBy(parcel => parcel.Priority);
                List<IDAL.DO.Parcel> gNormal = new List<IDAL.DO.Parcel>();
                List<IDAL.DO.Parcel> gExpress = new List<IDAL.DO.Parcel>();
                List<IDAL.DO.Parcel> gEmergency = new List<IDAL.DO.Parcel>();
                foreach (IGrouping<Priorities, IDAL.DO.Parcel> group in groups)
                {
                    if (group.Key == (Priorities)0)
                        gNormal = group.ToList();
                    else
                        if (group.Key == (Priorities)1)
                        gExpress = group.ToList();
                    else
                        gEmergency = group.ToList();
                }
                chosenParcel = GetParcelToDrone(gEmergency, droneList);
                if (chosenParcel.CodeParcel == default)
                {
                    chosenParcel = GetParcelToDrone(gExpress, droneList);
                    if (chosenParcel.CodeParcel == default)
                    {
                        chosenParcel = GetParcelToDrone(gExpress, droneList);
                        if (chosenParcel.CodeParcel == default)
                            throw new UpdateProblemException("Parcel wasn't found");
                    }
                }
                BODrones.Remove(droneList);
                droneList.DroneStatus =(DroneStatuses)2;
                BODrones.Add(droneList);
                chosenParcel.DroneId = id;
                chosenParcel.Scheduled = DateTime.Now;
                dl.UpDateParcel(chosenParcel);
            }
            throw new UpdateProblemException("This drone isn't free");

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
        public void UpdateDeliveredParcelByDrone(int id)
        {
            //update drone
            DroneList droneList = BODrones.ToList().Find(drone => drone.Id == id);
            if (droneList.Id == id)
            {
                IDAL.DO.Parcel parcel = dl.GetParcel(droneList.ParcelInWay);
                if (GetDateTimeToStatus(parcel) != (ParcelStatuses)2)
                    throw new UpdateProblemException("Impossible to deliver the parcel");
                else
                {
                    DroneList newDroneListnew = new DroneList
                    {
                        Id = droneList.Id,
                        ModelDrone = droneList.ModelDrone,
                        ParcelInWay = droneList.ParcelInWay,
                        Weight = droneList.Weight,
                        Battery = GetBatteryDeliveredParcel(droneList, parcel),
                        LocationNow = new Location(dl.GetCustomer(parcel.TargetId).Longitude, dl.GetCustomer(parcel.TargetId).Latitude),
                        DroneStatus = (DroneStatuses)0,
                    };
                    BODrones.ToList().Remove(droneList);
                    BODrones.ToList().Add(newDroneListnew);
                }
                //update parcel
                parcel.Delivered = DateTime.Now;
                dl.UpDateParcel(parcel);
            }
            throw new UpdateProblemException("The drone list doesn't exist");
        }
    }
}
        #endregion
    

