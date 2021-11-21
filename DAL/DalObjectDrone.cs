using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalObject
{
    public partial class DalObject
    {
        /// <summary>
        /// the function check if this station is exist yet
        /// </summary>
        /// <param name="id"></param>
        /// <returns> true if it is exist and else false</returns>
        public bool CheckDrone(int id)
        {
            return DataSource.drones.Any(drone => drone.CodeDrone == id);
        }
        /// <summary>
        /// the function gets an object of drone and adds it to the list of drones
        /// </summary>
        /// <param name="d"> a drone</param>
        public void AddDrone(Drone d)
        {
            if (CheckDrone(d.CodeDrone))
                throw new AlreadyExistException("The drone already exist in the system");
            DataSource.drones.Add(d);
        }
        /// <summary>
        /// the function searches the drone with the id that it got
        /// </summary>
        /// <param name="id"></param>
        /// <returns>the details of this drone</returns>
        public Drone GetDrone(int id)
        {
            Drone d = DataSource.drones.Find(drone => drone.CodeDrone == id);
            return d;
        }
        /// <summary>
        /// the function show the list of drones
        /// </summary>
        /// <returns>list of drones</returns>
        public IEnumerable<Drone> GetDrones()
        {
            return from drone in DataSource.drones select drone;
        }
    }
}
