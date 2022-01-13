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
        /// <summary>
        /// the function checks the details and throw errors in step and then adds a new drone to the list of drones
        /// </summary>
        /// <param name="d"> a new drone</param>
        public void AddDrone(Drone d, int idStation)
        {
            if (d.Id < 0)
                throw new AddingProblemException("The ID number must be positive");
            if ((int)d.MaxWeight < 0 || (int)d.MaxWeight > 2)
                throw new AddingProblemException("The max weight isn't valid");
            DO.BaseStation tempB = dal.GetStation(idStation);
            if (tempB.CodeStation != idStation)
                throw new AddingProblemException("The station doesn't exist");
            if (tempB.ChargeSlots <= 0)
                throw new AddingProblemException("Sorry, there are no charging slots available at the base station you selected.\n Please try another base station.");
            d.Battery = rnd.Next(20, 41);
            d.DroneStatus = DroneStatuses.maintenace;
            d.LocationNow = new Location(tempB.Longitude, tempB.Latitude);
            try
            {
                DO.Drone doDrone = new DO.Drone()
                {
                    CodeDrone = d.Id,
                    MaxWeight = (DO.WeightCategories)d.MaxWeight,
                    ModelDrone = d.ModelDrone,
                };
                dal.AddDrone(doDrone);
                tempB.ChargeSlots--;
                dal.UpDateBaseStation(tempB);
                dal.AddDroneCharge(d.Id, idStation,DateTime.Now);
                //add this drone to the bo drones
                BODrones.Add(new DroneForList()
                {
                    Id = d.Id,
                    ModelDrone=d.ModelDrone,
                    Weight=d.MaxWeight,
                    Battery=d.Battery,
                    DroneStatus=d.DroneStatus,
                    LocationNow=d.LocationNow,
                    ParcelInWay=d.ParcelInWay.Id
                }) ;
            }

            catch (Exception ex)
            {
                throw new AddingProblemException(ex.Message, ex);
            }
        }
        /// <summary>
        /// the funcrion gets new details of drone, checks them and throw matching errors
        /// In addition it calls to the function that update the drone from dal with the new details
        /// </summary>
        /// <param name="id">id of drone</param>
        /// <param name="model"> model of drone</param>
        public void UpdateDrone(Drone myDrone)
        {
            if (myDrone.Id < 0)
                throw new UpdateProblemException("The ID number must be a positive number");
            try
            {
                DO.Drone tempD = dal.GetDrone(myDrone.Id);
                tempD.ModelDrone = myDrone.ModelDrone;
                dal.UpDateDrone(tempD);
                //update this drone in the bo drones
                DroneForList boDrone = BODrones.Find(drone => drone.Id == myDrone.Id);
                BODrones.Remove(boDrone);
                boDrone.ModelDrone = myDrone.ModelDrone;
                BODrones.Add(boDrone);
            }
            catch (Exception ex)
            {
                throw new UpdateProblemException(ex.Message, ex);
            }
        }
        /// <summary>
        /// the function brings from the list of drones in bl and create new dron by this information
        /// </summary>
        /// <param name="id">id of drone</param>
        /// <returns>drone</returns>
        public Drone GetDrone(int id)
        {
            try
            {
                DO.Drone dalDrone = dal.GetDrone(id);
                DroneForList boDrone = BODrones.Find(boDrone => boDrone.Id == id);
                ParcelInWay p = new ParcelInWay();
                if (boDrone.ParcelInWay != 0)
                {
                    DO.Parcel dalParcel = dal.GetParcelsByCondition().ToList().Find(p => p.DroneId == id);
                    DO.Customer dalSender = dal.GetCustomer(dalParcel.SenderId);
                    DO.Customer dalTarget = dal.GetCustomer(dalParcel.TargetId);
                    Location locationS = new Location(dalSender.Longitude, dalSender.Longitude);
                    Location locationT = new Location(dalTarget.Longitude, dalTarget.Longitude);
                    bool pIsInWay;
                    if (dalParcel.PickedUp == null)
                        pIsInWay = false;
                    else
                        pIsInWay = true;
                    p = new ParcelInWay
                    {
                        Id = dalParcel.CodeParcel,
                        Priority = (Priorities)dalParcel.Priority,
                        Weight = (WeightCategories)dalParcel.Weight,
                        Sender = new CustomerInParcel() { Id = dalParcel.SenderId, Name = dal.GetCustomer(dalParcel.SenderId).NameCustomer },
                        Target = new CustomerInParcel() { Id = dalParcel.TargetId, Name = dal.GetCustomer(dalParcel.TargetId).NameCustomer },
                        IsInWay = pIsInWay,
                        LocationPickedUp = locationS,
                        LocationTarget = locationT,
                        TransportDistance = GetDistance(locationS, locationT)
                    };
                }
                return new Drone
                {
                    Id = dalDrone.CodeDrone,
                    ModelDrone = dalDrone.ModelDrone,
                    DroneStatus = boDrone.DroneStatus,
                    MaxWeight = (WeightCategories)dalDrone.MaxWeight,
                    Battery = boDrone.Battery,
                    LocationNow = boDrone.LocationNow,
                    ParcelInWay = p
                };
            }
            catch (Exception ex)
            {
                throw new GetDetailsProblemException(ex.Message, ex);
            }
        }
        /// <summary>
        /// the function returns the list of drones that we update in this program
        /// </summary>
        /// <returns>list of drones</returns>
        public IEnumerable<DroneForList> GetDrones()
        {
            return BODrones;
        }
        public IEnumerable<DroneForList> GetDronesByStatus(int num)
        {
            return from drone in BODrones
                   where (int)drone.DroneStatus == num
                   select drone;
        }
        public IEnumerable<DroneForList> GetDronesByWeight(int num)
        {
            return from drone in BODrones
                   where (int)drone.Weight == num
                   select drone;
        }
    }
}
