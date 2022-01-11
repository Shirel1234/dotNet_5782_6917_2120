using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using DalApi;
using BO;

namespace BL
{
    sealed partial class BL : IBL
    {
        #region singleton
        static readonly IBL instance = new BL();
        public static IBL Instance { get => instance; }

        internal IDal dal = DalFactory.GetDal();
        double free;
        double easy;
        double medium;
        double heavy;
        double chargingRate;

        #region
        public static Random rnd = new Random();
        
        List<DroneForList> BODrones = new List<DroneForList>();
        BL()
        {
            //dl = new DalObject.DalObject();
            double[] arr = dal.AskElectrical();
            free = arr[0];
            easy = arr[1];
            medium = arr[2];
            heavy = arr[3];
            chargingRate = arr[4];
            List<DO.Parcel> listParcels = dal.GetParcelsByCondition().ToList();
            IEnumerable<DroneForList> tempBoDrones = from DO.Drone item in dal.GetDronesByCondition().ToList()
                                                     let updatedDrone= GetUpdatedDetailDrone(item)
                                                  select new DroneForList()
                                                  {
                                                      Id = item.CodeDrone,
                                                      ModelDrone = item.ModelDrone,
                                                      Weight = (WeightCategories)item.MaxWeight,
                                                      DroneStatus = updatedDrone.DroneStatus,
                                                      LocationNow = updatedDrone.LocationNow,
                                                      Battery = updatedDrone.Battery,
                                                      ParcelInWay = 0
                                                  };
            BODrones = tempBoDrones.ToList();
        }
        /// <summary>
        /// the function finds the details of this drone and sending it to charge in the closer base station
        /// </summary>
        /// <param name="id"> id of drone</param>
        public void UpdateSendingDroneToCharge(int id)
        {
            DroneForList droneList = BODrones.Find(droneL => droneL.Id == id);
            if (droneList.Id == 0)
                throw new UpdateProblemException("This drone doesn't exist");
            if (droneList.DroneStatus == DroneStatuses.free)
            {
                DO.BaseStation closeStation = GetCloserBaseStation(droneList.LocationNow);
                double minDistance = GetDistance(new Location(closeStation.Longitude, closeStation.Latitude), droneList.LocationNow);
                if (droneList.Battery >= minDistance * free && closeStation.ChargeSlots > 0)
                {
                    //update drone
                    BODrones.Remove(droneList);
                    droneList.Battery -= minDistance * free;
                    droneList.LocationNow.Longitude = closeStation.Longitude;
                    droneList.LocationNow.Latitude = closeStation.Latitude;
                    droneList.DroneStatus = DroneStatuses.maintenace;
                    BODrones.Add(droneList);
                    //update station
                    closeStation.ChargeSlots--;
                    dal.UpDateBaseStation(closeStation);
                    //update drone Charge
                    //DroneCharge droneCharge = new DroneCharge() { Battery = droneList.Battery, Id = droneList.Id };
                    dal.AddDroneCharge(droneList.Id, closeStation.CodeStation, DateTime.Now);
                }
                else
                    throw new UpdateProblemException("It is impossible to send this drone to charging");
            }
            else
                throw new UpdateProblemException("The drone isn't free for charging");
        }
        public void UpdateReleasingDroneFromCharge(int id/*, double timeOfCharging*/)
        {
            DroneForList droneList = BODrones.Find(droneL => droneL.Id == id);
            if (droneList.Id == 0)
                throw new UpdateProblemException("This drone doesn't exist");
            if (droneList.DroneStatus == DroneStatuses.maintenace)
            {
                //update drone
                BODrones.Remove(droneList);
                droneList.Battery = Convert.ToDouble(dal.GetDroneCharge(id).BeginingCharge-DateTime.Now) * chargingRate;
                droneList.DroneStatus = DroneStatuses.free;
                BODrones.Add(droneList);
                //update station
                DO.BaseStation myStation = dal.GetStationsByCondition().ToList().Find(station => station.Longitude == droneList.LocationNow.Longitude && station.Latitude == droneList.LocationNow.Latitude);
                myStation.ChargeSlots++;
                dal.UpDateBaseStation(myStation);
                dal.RemoveDroneCharge(dal.GetDroneCharge(droneList.Id));
            }

        }
        public void UpdateParcelToDrone(int id)
        {
            DO.Parcel chosenParcel = default;
            //bring the details of this drone
            DroneForList droneList = BODrones.Find(droneL => droneL.Id == id);
            if (droneList.Id == 0)
                throw new UpdateProblemException("This drone doesn't exist");
            //check that the drone is free
            if (droneList.DroneStatus == DroneStatuses.free)
            {
                //create three groops by the type of priority
                var groups = dal.GetParcelsByCondition().ToList().GroupBy(parcel => parcel.Priority);
                List<DO.Parcel> gNormal = new List<DO.Parcel>();
                List<DO.Parcel> gExpress = new List<DO.Parcel>();

                List<DO.Parcel> gEmergency = new List<DO.Parcel>();
                foreach (IGrouping<DO.Priorities, DO.Parcel> group in groups)
                {
                    if (group.Key == DO.Priorities.normal)
                        gNormal = group.ToList();
                    else
                        if (group.Key == DO.Priorities.express)
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
                droneList.DroneStatus = DroneStatuses.sending;
                BODrones.Add(droneList);
                chosenParcel.DroneId = id;
                chosenParcel.Scheduled = DateTime.Now;
                dal.UpDateParcel(chosenParcel);
            }
            throw new UpdateProblemException("This drone isn't free");

        }
        public void UpdateParcelPickedUpByDrone(int droneID)
        {
            try
            {
                Drone drone = GetDrone(droneID);
                if (drone.DroneStatus == DroneStatuses.sending)
                {
                    DO.Parcel p = dal.GetParcelsByCondition().ToList().Find(parcel => parcel.DroneId == droneID);
                    //if the conditions match and the parcel was found
                    if (p.DroneId == droneID)
                    {
                        //update the drone and the parcel to be picked up by the drone 
                        Location senderLocation = new Location(dal.GetCustomer(p.SenderId).Longitude, dal.GetCustomer(p.SenderId).Latitude);
                        double distanceDroneToSender = GetDistance(drone.LocationNow, senderLocation);
                        DroneForList boDrone = BODrones.Find(drone => drone.Id == droneID);
                        BODrones.Remove(boDrone);
                        boDrone.Battery -= free * distanceDroneToSender;
                        boDrone.LocationNow = senderLocation;
                        BODrones.Add(boDrone);
                        p.PickedUp = DateTime.Now;
                        dal.UpDateParcel(p);
                        return;
                    }
                }
                throw new UpdateProblemException("This drone can't pick up the parcel");
            }
            catch
            {
                throw new UpdateProblemException();
            }
        }
        public void UpdateDeliveredParcelByDrone(int id)
        {
            try
            {
                //update drone
                DroneForList droneList = BODrones.ToList().Find(drone => drone.Id == id);
                if (droneList.Id == id)
                {
                    DO.Parcel parcel = dal.GetParcel(droneList.ParcelInWay);
                    if (GetDateTimeToStatus(parcel) != ParcelStatuses.pickedUp)
                        throw new UpdateProblemException("Impossible to deliver the parcel");
                    else
                    {
                        DroneForList newDroneListnew = new DroneForList
                        {
                            Id = droneList.Id,
                            ModelDrone = droneList.ModelDrone,
                            ParcelInWay = droneList.ParcelInWay,
                            Weight = droneList.Weight,
                            Battery = GetBatteryDeliveredParcel(droneList, parcel),
                            LocationNow = new Location(dal.GetCustomer(parcel.TargetId).Longitude, dal.GetCustomer(parcel.TargetId).Latitude),
                            DroneStatus = DroneStatuses.free,
                        };
                        BODrones.ToList().Remove(droneList);
                        BODrones.ToList().Add(newDroneListnew);
                    }
                    //update parcel
                    parcel.Delivered = DateTime.Now;
                    dal.UpDateParcel(parcel);
                }
                throw new UpdateProblemException("The drone list doesn't exist");
            }
            catch
            {
                throw new UpdateProblemException();
            }
        }
        #endregion
    }
}
        #endregion
    
    
