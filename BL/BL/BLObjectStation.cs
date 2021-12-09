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
        /// the function checks the details and throw errors in step and then adds a new station to the list of stations
        /// </summary>
        /// <param name="b"> a new station</param>
        public void AddBaseStation(BaseStation b) 
        {
                if (b.Id < 0)
                    throw new AddingProblemException("The ID number must be a positive number");
                if (b.ChargeSlots < 0)
                    throw new AddingProblemException("The number of the free charging positions must be a positive number");
                if (b.Location.Longitude < -180 || b.Location.Longitude > 180)
                    throw new AddingProblemException("The longitude must be between -180 to 180");
                if (b.Location.Latitude < -90 || b.Location.Latitude > 90)
                    throw new AddingProblemException("The latitude must be between -90 to 90");
            try
            {
                IDAL.DO.BaseStation doStation = new IDAL.DO.BaseStation()
                {
                    CodeStation = b.Id,
                    NameStation = b.Name,
                    ChargeSlots = b.ChargeSlots,
                    Longitude = b.Location.Longitude,
                    Latitude = b.Location.Latitude,
                };
                dl.AddStation(doStation);
            }
            catch (Exception ex)
            {
              throw new AddingProblemException("Can't add this baseStation", ex);
            }
        }
        /// <summary>
        /// the function gets new details of station, checks them and throw matching errors
        ///In addition it calls to the function that update the station from dal with the new details
        /// </summary>
        /// <param name="id">id of station</param>
        /// <param name="name">name of station</param>
        /// <param name="numOfChargePositions"> number of charge position</param>
        public void UpdateBaseStation(int id, int name, int numOfChargePositions)
        {
            if (id < 0)
                throw new UpdateProblemException("The ID number must be a positive number");
            if (numOfChargePositions < 0)
                throw new UpdateProblemException("The number of the charging positions must be a positive number");
            IDAL.DO.BaseStation tempB = dl.GetStation(id);
            if (name!=0)
                tempB.NameStation = name;
            if (numOfChargePositions != 0)
            {
                int busyChargingPositions = BODrones.Count
                    (drone => drone.DroneStatus == (DroneStatuses)1
                    && drone.LocationNow.Longitude == tempB.Longitude
                    && drone.LocationNow.Latitude == tempB.Latitude);
                tempB.ChargeSlots = numOfChargePositions- busyChargingPositions;
            }
            try
            {
                dl.UpDateBaseStation(tempB);
            }
            catch (Exception)
            {
                throw new UpdateProblemException();
            }
        }
        /// <summary>
        /// the function brings from the dal the details of the station and creates by it a station of bl
        /// </summary>
        /// <param name="id">id of base station</param>
        /// <returns>base station</returns>
        public BaseStation GetBaseStation(int id)
        {
            try
            {
                IDAL.DO.BaseStation dalStation = dl.GetStation(id);
                return new BaseStation()
                {
                    Id = dalStation.CodeStation,
                    Name = dalStation.NameStation,
                    ChargeSlots = dalStation.ChargeSlots,
                    Location = new Location(dalStation.Longitude, dalStation.Latitude),
                    ListDroneCharge = GetChargesDrone(dalStation)
                };
            }
            catch(Exception ex)
            {
                throw new GetDetailsProblemException("This drone does not exist");
            }
        }
        public IEnumerable<BaseStationList> GetAllBaseStations()
        {
            List<IDAL.DO.BaseStation> DOstatinsList = dl.GetBaseStations().ToList();
            return  
                from station in DOstatinsList
                 let busyChargingPositions = BODrones.Count
                    (drone => drone.DroneStatus == (DroneStatuses)1
                    && drone.LocationNow.Longitude == station.Longitude
                    && drone.LocationNow.Latitude == station.Latitude)
                 select new BaseStationList()
                 {
                     Id = station.CodeStation,
                     Name = station.NameStation,
                     ChargeSlotsFree = station.ChargeSlots,
                     ChargeSlotsCatch = busyChargingPositions
                 };
        }
        public IEnumerable<BaseStationList> GetAllBaseStationsWithChargePositions()
        {
            List<IDAL.DO.BaseStation> DOstatinsWithChargeSlotsList = dl.GetBaseStations().ToList().FindAll(station => station.ChargeSlots > 0);
            return
               from station in DOstatinsWithChargeSlotsList
               let busyChargingPositions = BODrones.Count
                   (drone => drone.DroneStatus == (DroneStatuses)1
                   && drone.LocationNow.Longitude == station.Longitude
                   && drone.LocationNow.Latitude == station.Latitude)
               select new BaseStationList()
               {
                   Id = station.CodeStation,
                   Name = station.NameStation,
                   ChargeSlotsFree = station.ChargeSlots,
                   ChargeSlotsCatch = busyChargingPositions
               };
        }
        public IEnumerable<DroneCharge> GetChargesDrone(IDAL.DO.BaseStation dalStation)
        {
            IEnumerable<DroneCharge> dronesCharge = from boDrone in BODrones
                                                    where boDrone.DroneStatus == DroneStatuses.maintenace && boDrone.LocationNow.Longitude == dalStation.Longitude && boDrone.LocationNow.Latitude == dalStation.Latitude
                                                    select new DroneCharge
                                                    {
                                                        Battery = boDrone.Battery,
                                                        Id = boDrone.Id,
                                                    };
            if (dronesCharge.Count() == 0)
                dronesCharge = new List<DroneCharge>();
            return dronesCharge;
        }

    }
}
