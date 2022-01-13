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
        #endregion
        internal IDal dal = DalFactory.GetDal();
        double free;
        double easy;
        double medium;
        double heavy;
        double chargingRate;
        #region create Drone List
        public static Random rnd = new Random();
        
        public List<DroneForList> BODrones = new List<DroneForList>();
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
                                                      ParcelInWay = updatedDrone.ParcelInWay
                                                  };
            BODrones = tempBoDrones.ToList();
        }
        #endregion

        #region Updating
        /// <summary>
        /// the function finds the details of this drone and sending it to charge in the closer base station
        /// </summary>
        /// <param name="id"> id of drone</param>
        public void UpdateSendingDroneToCharge(int id)
        {
            DroneForList drone = BODrones.Find(droneL => droneL.Id == id);
            if (drone.Id == 0)
                throw new UpdateProblemException("This drone doesn't exist");
            if (drone.DroneStatus == DroneStatuses.free || drone.DroneStatus == DroneStatuses.maintenace)
            {
                DO.BaseStation closeStation = GetCloserBaseStation(drone.LocationNow);
                double minDistance = GetDistance(new Location(closeStation.Longitude, closeStation.Latitude), drone.LocationNow);
                if (drone.Battery >= minDistance * free) 
                {
                    if(closeStation.ChargeSlots > 0)
                    {
                        //update drone
                        BODrones.Remove(drone);
                        drone.Battery -= minDistance * free;
                        drone.LocationNow.Longitude = closeStation.Longitude;
                        drone.LocationNow.Latitude = closeStation.Latitude;
                        drone.DroneStatus = DroneStatuses.maintenace;
                        BODrones.Add(drone);
                        //update station
                        closeStation.ChargeSlots--;
                        dal.UpDateBaseStation(closeStation);
                        //update drone Charge
                        //DroneCharge droneCharge = new DroneCharge() { Battery = droneList.Battery, Id = droneList.Id };
                        dal.AddDroneCharge(drone.Id, closeStation.CodeStation, DateTime.Now);
                    }
                    else
                        throw new UpdateProblemException("It is impossible to send this drone to charging.\nThere are not available charge slots in the closer station.");
                }
                else
                    throw new UpdateProblemException("It is impossible to send this drone to charging.\nThere drone does not have enough battery to reach the closer station.");
            }
            else
                throw new UpdateProblemException("The drone isn't free for charging");
        }
        /// <summary>
        /// the function releasesthe drone fron charging and calculates the battery after the charging by the timethat passed
        /// </summary>
        /// <param name="id"> id of drone </param>
        public void UpdateReleasingDroneFromCharge(int id)
        {
            DroneForList drone = BODrones.Find(droneL => droneL.Id == id);
            if (drone.Id == 0)
                throw new UpdateProblemException("This drone doesn't exist");
            try
            {
                if (drone.DroneStatus == DroneStatuses.maintenace)
                {
                    //update drone
                    BODrones.Remove(drone);
                    TimeSpan timeCharge = (TimeSpan)(DateTime.Now - dal.GetDroneCharge(id).BeginingCharge);
                    double battery = Convert.ToDouble(timeCharge.Seconds) * chargingRate + drone.Battery;
                    if (battery > 100)
                        drone.Battery = 100;
                    else
                        drone.Battery = battery;

                    drone.DroneStatus = DroneStatuses.free;
                    BODrones.Add(drone);
                    DO.DroneCharge dc = dal.GetDroneCharge(drone.Id);
                    //update station
                    DO.BaseStation myStation = dal.GetStationsByCondition().ToList().Find(station => station.CodeStation==dc.StationID);
                    myStation.ChargeSlots++;
                    dal.UpDateBaseStation(myStation);
                    dal.RemoveDroneCharge(dal.GetDroneCharge(drone.Id));
                } 
            }
            catch(Exception ex)
            {
                throw new UpdateProblemException(ex.Message);
            }
        }
        /// <summary>
        /// the function search thr match parcel for the drone by priority
        /// </summary>
        /// <param name="id">id of drone </param>
        /// <returns></returns>
        public bool UpdateParcelToDrone(int id)
        {
            DO.Parcel chosenParcel = default;
            //bring the details of this drone
            DroneForList droneList = BODrones.Find(droneL => droneL.Id == id);
            if (droneList.Id == 0)
                throw new UpdateProblemException("This drone doesn't exist");
            try
            {
                //check that the drone is free
                if (droneList.DroneStatus == DroneStatuses.free)
                {
                    //create three groups by the type of priority
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
                    if (GetBatteryDeliveredParcel(droneList, chosenParcel) > droneList.Battery)
                        return false;
                    BODrones.Remove(droneList);
                    droneList.DroneStatus = DroneStatuses.sending;
                    BODrones.Add(droneList);
                    chosenParcel.DroneId = id;
                    chosenParcel.Scheduled = DateTime.Now;
                    dal.UpDateParcel(chosenParcel);
                    return true;
                }
                throw new UpdateProblemException("This drone isn't free");
            }
            catch (Exception ex)
            {
                throw new UpdateProblemException(ex.Message);
            }
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
                DroneForList drone = BODrones.ToList().Find(drone => drone.Id == id);
                if (drone.Id == id)
                {
                    DO.Parcel parcel = dal.GetParcel(drone.ParcelInWay);
                    if (GetDateTimeToStatus(parcel) != ParcelStatuses.pickedUp)
                        throw new UpdateProblemException("Impossible to deliver the parcel");
                    else
                    {
                        DroneForList newDrone = new DroneForList
                        {
                            Id = drone.Id,
                            ModelDrone = drone.ModelDrone,
                            ParcelInWay = 0,
                            Weight = drone.Weight,
                            Battery = GetBatteryDeliveredParcel(drone, parcel),
                            LocationNow = new Location(dal.GetCustomer(parcel.TargetId).Longitude, dal.GetCustomer(parcel.TargetId).Latitude),
                            DroneStatus = DroneStatuses.free,
                        };
                        BODrones.ToList().Remove(drone);
                        BODrones.ToList().Add(newDrone);
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
        #region Simulator
        public void StartSimulator(int id, Action updateDelegate, Func<bool> stopDelegate)
        {
            new Simulator(id, updateDelegate, stopDelegate, this);
        }
        #endregion

    }
}    

