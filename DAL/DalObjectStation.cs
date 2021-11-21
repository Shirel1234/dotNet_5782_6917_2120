using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DalObject
{
    public partial class DalObject: IDal
    {
        /// <summary>
        /// the function check if this station is exist yet
        /// </summary>
        /// <param name="id"></param>
        /// <returns> true if it is exist and else false</returns>
        public bool CheckStation(int id)
        { 
            return DataSource.stations.Any(station=> station.CodeStation == id);
        }
        /// <summary>
        /// the function gets an object of base station and adds it to the list of base stations
        /// </summary>
        /// <param name="b"></param>
        public void AddStation(BaseStation b)
        {
            if (CheckStation(b.CodeStation))
                throw new AlreadyExistException("The baseStation already exist in the system");
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
            return b;
        }
        /// <summary>
        /// the function show the list of stations
        /// </summary>
        /// <returns>list of stations</returns>
        public IEnumerable<BaseStation>  GetBaseStations()
        {
            return from station in DataSource.stations select station;
        }

    }
}

