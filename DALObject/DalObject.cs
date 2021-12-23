using System;
using DalApi;
using System.Collections.Generic;
using DO;

namespace Dal
{
    sealed partial class DalObject : IDal
    {
        static readonly IDal instance = new DalObject();
        public static IDal Instance { get => instance; }
        DalObject() 
        {
            DataSource.Initialize();
        }
        /// <summary>
        /// the function looks for this parcel in the list 
        /// and updates its drone field to be the drone the function got 
        /// </summary>
        /// <param name="idP"> an id of parcel</param>
        /// <param name="idD">an id of drone</param>      
        public void UpdateParcelToDrone(int idP, int idD)
        {
            List<Parcel> parcels = new List<Parcel>();
            Parcel p = DataSource.parcels.Find(parcel => parcel.CodeParcel == idP);
            if (p.CodeParcel == idP)
                throw new DoesntExistException("parcel does not exist");
            p.DroneId = idD;
            p.Requested = DateTime.Now;
        }
        /// <summary>
        /// the function searchs the parcel with this id  and updates the parcel to be pick up
        /// </summary>
        /// <param name="idP"> an id of parcel</param>
        public void UpdatePickedUp(int idP)
        {
            List<Parcel> parcels = new List<Parcel>();
            Parcel p = DataSource.parcels.Find(parcel => parcel.CodeParcel == idP);
            if (p.CodeParcel == idP)
                throw new DoesntExistException("parcel does not exist");
            p.PickedUp = DateTime.Now;
        }
        /// <summary>
        /// the function searches the parcel with this id and updates the parcel to be deliver
        /// </summary>
        /// <param name="idP"> an id of parcel</param>
        public void UpdateDeliver(int idP)
        {
            //List<Parcel> parcels = new();
            Parcel p = DataSource.parcels.Find(parcel => parcel.CodeParcel == idP);
            if (p.CodeParcel == idP)
                throw new DoesntExistException("parcel does not exist");
            p.Delivered = DateTime.Now;
        }
        /// <summary>
        /// the function searches the drone with this id and updates its status to be maintenace
        /// in additions it creates a droneCharge and uptates its id of drone and id of station
        /// </summary>
        /// <param name="idD"> an id of drone</param>
        /// <param name="idS">an id of station</param>
        public void UpDateCharge(int idD, int idS)
        {
            Drone d = DataSource.drones.Find(drone => drone.CodeDrone == idD);
            if (d.CodeDrone == idD)
                throw new DoesntExistException("drone does not exist");
            DroneCharge dc = new DroneCharge();
            dc.DroneID = idD;
            dc.StationID = idS;
        }
        ///<summary>
        /// the function searches the drone with this id and updates its status to be free   
        /// </summary>
        /// <param name="idD"> an id of drone</param>
        /// <param name="idS">an id of station</param>
        public void ReleaseCharge(int idD, int idS)
        {
            Drone d = DataSource.drones.Find(drone => drone.CodeDrone == idD);
            if (d.CodeDrone == idD)
                throw new DoesntExistException("drone does not exist");
        }
        /// <summary>
        /// the function searches the parcels that have not drone
        /// </summary>
        /// <returns></returns>
        //public IEnumerable<Parcel> GetParcels(Predicate<Parcel> conditionDelegate)
        //{
        //    List<Parcel> lstParcelWithoutDrone = DataSource.parcels.FindAll(parcel => conditionDelegate(parcel));
        //    return lstParcelWithoutDrone;//האם זה בסדר שיתכן ויחזיר רשימה ריקה, או שיש לבדוק זאת ולזרוק חריגה אם כן?כנ"ל לגבי כל המתודות לעיל המחזירות רשימות
        //}
        /// <summary>
        /// the function searches for the stations that have free slots of charge and creates a new list with them
        /// </summary>
        /// <returns>the new list of stations with free charge slots</returns>
        /// template<class T, class K>


        public DroneCharge GetDroneCharge(int idDC)
        {
            return DataSource.dronesCharge.Find(droneCharge => droneCharge.DroneID == idDC);
        }
        public void AddDroneCharge(int idDrone, int idStation, DateTime? dateTimeBegining)
        {
            DroneCharge dc = new DroneCharge()
            {
                DroneID = idDrone,
                StationID = idStation,
                BeginingCharge = dateTimeBegining
            };
            DataSource.dronesCharge.Add(dc);
        }
        public void RemoveDroneCharge(DroneCharge dc)
        {
            DataSource.dronesCharge.Remove(dc);
        }
        public double[] AskElectrical()
        {
            double[] arrElectrical = new double[5];
            arrElectrical[0] = DataSource.Config.Free;
            arrElectrical[1] = DataSource.Config.Easy;
            arrElectrical[2] = DataSource.Config.Medium;
            arrElectrical[3] = DataSource.Config.Heavy;
            arrElectrical[4] = DataSource.Config.ChargingRate;
            return arrElectrical;
        }
    }
}
