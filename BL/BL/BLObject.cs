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
                var matchWeightParcels = from item in gEmergency
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
        public void UpdateParcelPickedUpByDrone(int idD)
        {

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
                        Battery = GetBatteryDeliveredParccel(droneList, parcel),
                        LocationNow = new Location(dl.GetCustomer(parcel.TargetId).Longitude, dl.GetCustomer(parcel.TargetId).Latitude),
                        DroneStatus = (DroneStatuses)0,
                    };
                    BODrones.ToList().Remove(droneList);
                    BODrones.ToList().Add(newDroneListnew);

                }
                //update parcel
                IDAL.DO.Parcel newParcel = new IDAL.DO.Parcel
                {
                    CodeParcel = parcel.CodeParcel,
                    Weight = parcel.Weight,
                    Priority = parcel.Priority,
                    SenderId = parcel.SenderId,
                    TargetId = parcel.TargetId,
                    DroneId = parcel.DroneId,
                    Requested = parcel.Requested,
                    Scheduled = parcel.Scheduled,
                    PickedUp = parcel.PickedUp,
                    Delivered = DateTime.Now
                };

            }
            throw new UpdateProblemException("The drone list doesn't exist");
        }
    }
}
        #endregion
    

