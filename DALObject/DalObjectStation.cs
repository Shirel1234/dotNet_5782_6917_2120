﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;


namespace Dal
{
    partial class DalObject
    {
        #region
        /// <summary>
        /// the function check if this station is exist yet
        /// </summary>
        /// <param name="id"></param>
        /// <returns> true if it is exist and else false</returns>
        public bool CheckStation(int id)
        {
            return DataSource.stations.Any(station => station.CodeStation == id);
        }
        /// <summary>
        /// the function gets an object of base station and adds it to the list of base stations
        /// </summary>
        /// <param name="b"></param>
        public void AddStation(BaseStation b)
        {
            if (CheckStation(b.CodeStation))
                throw new AlreadyExistException("The base station already exist in the system");
            DataSource.stations.Add(b);
        }
        public void UpDateBaseStation(BaseStation b)
        {
            BaseStation myStation = DataSource.stations.Find(x => x.CodeStation == b.CodeStation);
            if (myStation.CodeStation != b.CodeStation)
                throw new DoesntExistException("This base station doesn't exist in the system");
            DataSource.stations.Remove(myStation);
            DataSource.stations.Add(b);
        }
        /// <summary>
        /// the function searches the station with the id that it got
        /// </summary>
        /// <param name="id"></param>
        /// <returns>the details of this station</returns>
        public BaseStation GetStation(int id)
        {
            BaseStation b = DataSource.stations.Find(station => station.CodeStation == id);
            if (b.CodeStation == id)
                return b;
            throw new DoesntExistException("This station does not exist");
        }

        /// <summary>
        /// the function show the list of stations
        /// </summary>
        /// <returns>list of stations</returns>
        public IEnumerable<BaseStation> GetStationsByCondition(Func<BaseStation, bool> conditionDelegate = null)
        {
            if (conditionDelegate == null)
                return from station in DataSource.stations select station;
            else
            {
                List<BaseStation> lstStationsWithSlots = DataSource.stations.FindAll(station => conditionDelegate(station));
                return lstStationsWithSlots;
            }
        }
        #endregion
    }
}
