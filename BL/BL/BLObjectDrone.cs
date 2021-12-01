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
        Random rnd = new Random();
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
        public IEnumerable<DroneList> GetAllDrones()
        {
            return BODrones;
        }
    }
}