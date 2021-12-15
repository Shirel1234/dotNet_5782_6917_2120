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
                throw new AlreadyExistException("This drone already exist in the system");
            DataSource.drones.Add(d);
        }
        public void UpDateDrone(Drone d)
        {
            Drone myDrone = DataSource.drones.Find(x => x.CodeDrone == d.CodeDrone);
            if (myDrone.CodeDrone != d.CodeDrone)
                throw new DoesntExistException("This drone doesn't exist in the system");
            //myDrone.CodeDrone = d.CodeDrone;
            //myDrone.ModelDrone = d.ModelDrone;
            //myDrone.MaxWeight = d.MaxWeight;
            DataSource.drones.Remove(myDrone);
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
            if(d.CodeDrone==id)
                return d;
            throw new DoesntExistException("This drone does not exist");
        }
        /// <summary>
        /// the function show the list of drones
        /// </summary>
        /// <returns>list of drones</returns>
        public IEnumerable<Drone> GetDronesByCondition(Func<Drone, bool> conditionDelegate = null)
        {
            if (conditionDelegate == null)
                return from drone in DataSource.drones select drone;
            else
            {
                List<Drone> listDronesByCondition = DataSource.drones.FindAll(drone => conditionDelegate(drone));
                return listDronesByCondition;
            }
        }
    }
}
