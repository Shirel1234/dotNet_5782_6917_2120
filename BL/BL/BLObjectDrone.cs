using DalObject;
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
        public void AddDrone(Drone d, int idStation)
        {
            if (d.Id < 0)
                throw new AddingProblemException("The ID number must be positive");
            if (d.MaxWeight < 0)///operator>
                throw new AddingProblemException("The max weight isn't valid");
            IDAL.DO.BaseStation tempB = dl.GetBaseStations().ToList().Find(station => station.CodeStation == idStation);
            if (tempB.CodeStation != idStation)
                throw new AddingProblemException("The station doesn't exist");
            d.Battery = rnd.Next(20, 41);
            d.DroneStatus = (DroneStatuses)1;
            d.LocationNow.Longitude = tempB.Longitude;
            d.LocationNow.Latitude = tempB.Latitude;
            try
            {
                IDAL.DO.Drone doDrone = new IDAL.DO.Drone()
                {
                    CodeDrone = d.Id,
                    MaxWeight = (IDAL.DO.WeightCategories)d.MaxWeight,
                    ModelDrone = d.ModelDrone,
                };
                dl.AddDrone(doDrone);
            }

            catch (Exception ex)
            {
                throw new AddingProblemException("Can't add this drone", ex);//למה שלא נזרוק הלאה את החריגה שכבר קיבלנו???
            }
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
        public void UpdateDrone(int id, string model)
        {
            if (id < 0)
                throw new UpdateProblemException("The ID number must be a positive number");
            IDAL.DO.Drone tempD = dl.GetDrone(id);
            tempD.ModelDrone = model;
            try
            {
                dl.UpDateDrone(tempD);
            }
            catch (Exception)
            {
                throw new UpdateProblemException();
            }
        }
        public Drone GetDrone(int id)
        {
            IDAL.DO.Drone dalDrone = dl.GetDrone(id);
            DroneList boDrone = (DroneList)(from drone in BODrones
                                            where drone.Id == id
                                            select drone);
            IDAL.DO.Parcel dalParcel = dl.GetParcels().ToList().Find(p => p.DroneId == id);
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
                    SenderId = dalParcel.SenderId,
                    TargetId = dalParcel.TargetId,
                    IsInWay = true,
                    LocationPickedUp = locationS,
                    LocationTarget = locationT,
                    TransportDistance = HelpClass.GetDistance(locationS, locationT)
                }

            };
        }
        public IEnumerable<DroneList> GetAllDrones()
        {
            return BODrones;
        }

        /// <summary>
        /// the function show the list of stations
        /// </summary>
        /// <returns>list of stations</returns>

    }
}

