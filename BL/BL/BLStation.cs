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
        /// the function checks the details and throw errors in step and then adds a new station to the list of stations
        /// </summary>
        /// <param name="b"> a new station</param>
        public void AddBaseStation(BaseStation b)
        {
            if (b.Id < 0)
                throw new AddingProblemException("The ID number must be a positive number");
            if (b.ChargeSlots < 0)
                throw new AddingProblemException("The number of the free charging positions must be a positive number");
            if (b.Location.Longitude < 29.3 || b.Location.Longitude > 33.7)
                throw new AddingProblemException("The longitude must be between 29.3 to 33.7");
            if (b.Location.Latitude < 33.5 || b.Location.Latitude > 36.3)
                throw new AddingProblemException("The latitude must be between 33.5 to 36.3");
            try
            {
                DO.BaseStation doStation = new DO.BaseStation()
                {
                    CodeStation = b.Id,
                    NameStation = b.Name,
                    ChargeSlots = b.ChargeSlots,
                    Longitude = b.Location.Longitude,
                    Latitude = b.Location.Latitude,
                };
                dal.AddStation(doStation);
            }
            catch (Exception ex)
            {
                throw new AddingProblemException(ex.Message, ex);
            }
        }
        /// <summary>
        /// the function gets new details of station, checks them and throw matching errors
        ///In addition it calls to the function that update the station from dal with the new details
        /// </summary>
        /// <param name="id">id of station</param>
        /// <param name="name">name of station</param>
        /// <param name="numOfChargePositions"> number of charge position</param>
        public void UpdateBaseStation(int id, int name, int numOfChargeSlots)
        {
            if (id < 0)
                throw new UpdateProblemException("The ID number must be a positive number");
            if (numOfChargeSlots < 0)
                throw new UpdateProblemException("The number of the charging positions must be a positive number");
            DO.BaseStation tempB = dal.GetStation(id);
            if (name != 0)
                tempB.NameStation = name;
            if (numOfChargeSlots >= 0)
            {
                int busyChargeSlots = BODrones.Count
                    (drone => drone.DroneStatus == DroneStatuses.maintenace
                    && drone.LocationNow.Longitude == tempB.Longitude
                    && drone.LocationNow.Latitude == tempB.Latitude);
                tempB.ChargeSlots = numOfChargeSlots - busyChargeSlots;
                if (tempB.ChargeSlots < 0)
                    throw new UpdateProblemException("ERROR. The number of the charge slots you entered is smaller than the number of the busy charge slots in this station.\nIt's impossible.");
            }
            try
            {
                dal.UpDateBaseStation(tempB);
            }
            catch (Exception ex)
            {
                throw new UpdateProblemException(ex.Message, ex);
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
                DO.BaseStation dalStation = dal.GetStation(id);
                return new BaseStation()
                {
                    Id = dalStation.CodeStation,
                    Name = dalStation.NameStation,
                    ChargeSlots = dalStation.ChargeSlots,
                    Location = new Location(dalStation.Longitude, dalStation.Latitude),
                    ListDroneCharge = GetChargesDrone(dalStation)
                };
            }
            catch (Exception ex)
            {
                throw new GetDetailsProblemException(ex.Message, ex);
            }
        }
        /// <summary>
        /// the function brings all the stations from the dal and adds the mess details for bl  station
        /// </summary>
        /// <returns>list of station</returns>
        public IEnumerable<BaseStationForList> GetBaseStations()
        {
            try
            {
                List<DO.BaseStation> DOstatinsList = dal.GetStationsByCondition().ToList();
                return
                    from station in DOstatinsList
                    let busyChargingPositions = BODrones.Count
                   (drone => drone.DroneStatus == DroneStatuses.maintenace
                   && drone.LocationNow.Longitude == station.Longitude
                   && drone.LocationNow.Latitude == station.Latitude)
                    select new BaseStationForList()
                    {
                        Id = station.CodeStation,
                        Name = station.NameStation,
                        ChargeSlotsFree = station.ChargeSlots,
                        ChargeSlotsCatch = busyChargingPositions
                    };
            }
            catch (Exception ex)
            {
                throw new GetDetailsProblemException(ex.Message, ex);
            }
        }
        /// <summary>
        /// the function create a list of all the station that are without charge position
        /// </summary>
        /// <returns>list of station without charge position</returns>
        public IEnumerable<BaseStationForList> GetAllBaseStationsWithChargePositions()
        {
            return from station in dal.GetStationsByCondition().ToList()
                   where station.ChargeSlots > 0
                   let busyChargingSlots = BODrones.Count
                                     (drone => drone.DroneStatus == DroneStatuses.maintenace
                                     && drone.LocationNow.Longitude == station.Longitude
                                     && drone.LocationNow.Latitude == station.Latitude)
                   select new BaseStationForList()
                   {
                       Id = station.CodeStation,
                       Name = station.NameStation,
                       ChargeSlotsFree = station.ChargeSlots,
                       ChargeSlotsCatch = busyChargingSlots
                   };

        }
        /// <summary>
        /// the function create a list of drones in the station that got
        /// </summary>
        /// <param name="dalStation"> station</param>
        /// <returns>list of drones in this station</returns>
        public IEnumerable<DroneInCharge> GetChargesDrone(DO.BaseStation dalStation)
        {
            IEnumerable<DO.DroneCharge> dalDronesCharge = dal.GetDronesChargeByCondition();
            IEnumerable<DroneInCharge> dronesCharge = from boDrone in BODrones
                                                      where boDrone.DroneStatus == DroneStatuses.maintenace && boDrone.LocationNow.Longitude == dalStation.Longitude && boDrone.LocationNow.Latitude == dalStation.Latitude
                                                      select new DroneInCharge
                                                      {
                                                          Battery = boDrone.Battery,
                                                          Id = boDrone.Id,
                                                          DateTimeBegining = dalDronesCharge.ToList().Find(dc => dc.DroneID == boDrone.Id).BeginingCharge
                                                    };
            if (dronesCharge.Count() == 0)
                dronesCharge = new List<DroneInCharge>();
            return dronesCharge;
        }
    }
}
