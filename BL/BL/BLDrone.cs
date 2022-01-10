﻿using System;
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
            }

            catch (Exception ex)
            {
                throw new AddingProblemException(ex.Message, ex);//למה שלא נזרוק הלאה את החריגה שכבר קיבלנו???
            }
            //add this drone to the bo drones
            DroneForList boDrone = new DroneForList()
            {
                Id = d.Id,
                ModelDrone = d.ModelDrone,
                Weight = d.MaxWeight,
                DroneStatus = d.DroneStatus,
                Battery = d.Battery,
                LocationNow = d.LocationNow,
                ParcelInWay = 0
            };
            BODrones.Add(boDrone);
        }
        /// <summary>
        /// the funcrion gets new details of drone, checks them and throw matching errors
        /// In addition it calls to the function that update the drone from dal with the new details
        /// </summary>
        /// <param name="id">id of drone</param>
        /// <param name="model"> model of drone</param>
        public void UpdateDrone(int id, string model)
        {
            if (id < 0)
                throw new UpdateProblemException("The ID number must be a positive number");
            try
            {
                DO.Drone tempD = dal.GetDrone(id);
                tempD.ModelDrone = model;
                dal.UpDateDrone(tempD);
            }
            catch (Exception ex)
            {
                throw new UpdateProblemException(ex.Message, ex);
            }
            DroneForList boDrone = BODrones.Find(drone => drone.Id == id);
            BODrones.Remove(boDrone);
            boDrone.ModelDrone = model;
            BODrones.Add(boDrone);
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
                DO.Parcel dalParcel = dal.GetParcelsByCondition().ToList().Find(p => p.DroneId == id);
                DO.Customer dalSender = dal.GetCustomer(dalParcel.SenderId);
                DO.Customer dalTarget = dal.GetCustomer(dalParcel.TargetId);
                Location locationS = new Location(dalSender.Longitude, dalSender.Longitude);
                Location locationT = new Location(dalTarget.Longitude, dalTarget.Longitude);
                return new Drone
                {
                    Id = dalDrone.CodeDrone,
                    ModelDrone = dalDrone.ModelDrone,
                    DroneStatus = boDrone.DroneStatus,
                    MaxWeight = (WeightCategories)dalDrone.MaxWeight,
                    Battery = boDrone.Battery,
                    LocationNow = boDrone.LocationNow,
                    ParcelInWay = new ParcelInWay
                    {
                        Id = dalParcel.CodeParcel,
                        Priority = (Priorities)dalParcel.Priority,
                        Weight = (WeightCategories)dalParcel.Weight,
                        Sender = new CustomerInParcel() { Id = dalParcel.SenderId, Name = dal.GetCustomer(dalParcel.SenderId).NameCustomer },
                        Target = new CustomerInParcel() { Id = dalParcel.TargetId, Name = dal.GetCustomer(dalParcel.TargetId).NameCustomer },
                        IsInWay = true,
                        LocationPickedUp = locationS,
                        LocationTarget = locationT,
                        TransportDistance = GetDistance(locationS, locationT)
                    }

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
