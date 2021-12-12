﻿using DalObject;
using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    public partial class BLObject
    {
        /// <summary>
        /// the function checks the details and throw errors in step and then adds a new drone to the list of drones
        /// </summary>
        /// <param name="d"> a new drone</param>
        public void AddDrone(Drone d, int idStation)
        {
            if (d.Id < 0)
                throw new AddingProblemException("The ID number must be positive");
            if ((int)d.MaxWeight < 0|| (int)d.MaxWeight>2)
                throw new AddingProblemException("The max weight isn't valid");
            IDAL.DO.BaseStation tempB = dl.GetStation(idStation);
            if (tempB.CodeStation != idStation)
                throw new AddingProblemException("The station doesn't exist");
            d.Battery = rnd.Next(20, 41);
            d.DroneStatus = (DroneStatuses)1;
            d.LocationNow= new Location(tempB.Longitude, tempB.Latitude);
            try
            {
                IDAL.DO.Drone doDrone = new IDAL.DO.Drone()
                {
                    CodeDrone = d.Id,
                    MaxWeight = (IDAL.DO.WeightCategories)d.MaxWeight,
                    ModelDrone = d.ModelDrone,
                };
                dl.AddDrone(doDrone);
                tempB.ChargeSlots--;
                dl.UpDateBaseStation(tempB);
                dl.AddDroneCharge(d.Id, idStation);
            }

            catch (Exception ex)
            {
                throw new AddingProblemException(ex.Message, ex);//למה שלא נזרוק הלאה את החריגה שכבר קיבלנו???
            }
            //add this drone to the bo drones
            DroneList boDrone = new DroneList()
            {
                Id=d.Id,
                ModelDrone=d.ModelDrone,
                Weight=d.MaxWeight,
                DroneStatus=d.DroneStatus,
                Battery=d.Battery,
                LocationNow=d.LocationNow,
                ParcelInWay=0
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
                IDAL.DO.Drone tempD = dl.GetDrone(id);
                tempD.ModelDrone = model;
                dl.UpDateDrone(tempD);
            }
            catch (Exception ex)
            {
                throw new UpdateProblemException(ex.Message,ex);
            }
            DroneList boDrone = BODrones.Find(drone => drone.Id == id);
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
                IDAL.DO.Drone dalDrone = dl.GetDrone(id);
                DroneList boDrone = BODrones.Find(boDrone => boDrone.Id == id);
                IDAL.DO.Parcel dalParcel = dl.GetParcelsByCondition().ToList().Find(p => p.DroneId == id);
                IDAL.DO.Customer dalSender = dl.GetCustomer(dalParcel.SenderId);
                IDAL.DO.Customer dalTarget = dl.GetCustomer(dalParcel.TargetId);
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
                        Sender = new CustomerParcel() { Id = dalParcel.SenderId, Name = dl.GetCustomer(dalParcel.SenderId).NameCustomer },
                        Target = new CustomerParcel() { Id = dalParcel.TargetId, Name = dl.GetCustomer(dalParcel.TargetId).NameCustomer },
                        IsInWay = true,
                        LocationPickedUp = locationS,
                        LocationTarget = locationT,
                        TransportDistance = GetDistance(locationS, locationT)
                    }

                };
            }
            catch(Exception ex)
            {
                throw new GetDetailsProblemException(ex.Message,ex);
            }
        }
        /// <summary>
        /// the function returns the list of drones that we update in this program
        /// </summary>
        /// <returns>list of drones</returns>
        public IEnumerable<DroneList> GetAllDrones()
        {
            return BODrones;
        }


    }
}

