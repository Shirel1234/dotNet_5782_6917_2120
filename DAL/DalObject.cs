using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DalObject
{
    public partial class DalObject : IDal
    {
        /// <summary>
        /// constructor of the class DalObject: it calls the function of initializing from DataSource.
        /// </summary>
        public DalObject()
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
            List<Parcel> parcels = new();
            Parcel p = DataSource.parcels.Find(parcel => parcel.CodeParcel == idP);
            if (p.CodeParcel == idP)
                throw new DoesntExistException("parcel does not exist");
            p.DroneId = idD;
            p.Requested = DateTime.Now;
        }
        //for (int i = 0; i < DataSource.parcels.Count; i++)
        //{
        //    if (DataSource.parcels[i].CodeParcel==idP)
        //    {
        //        Parcel p = new Parcel();
        //        p.CodeParcel = DataSource.parcels[i].CodeParcel;
        //        p.Delivered = DataSource.parcels[i].Delivered;
        //        p.DroneId = idD;
        //        p.PickedUp = DataSource.parcels[i].PickedUp;
        //        p.Priority = DataSource.parcels[i].Priority;
        //        p.Requested = DateTime.Now;
        //        p.Scheduled = DataSource.parcels[i].Scheduled;
        //        p.SenderId = DataSource.parcels[i].SenderId;
        //        p.TargetId = DataSource.parcels[i].TargetId;
        //        p.Weight = DataSource.parcels[i].Weight;
        //        DataSource.parcels[i] = p;
        //        break;
        //    }
        //}    
        /// <summary>
        /// the function searchs the parcel with this id  and updates the parcel to be pick up
        /// </summary>
        /// <param name="idP"> an id of parcel</param>
        public void UpdatePickedUp(int idP)
        {
            List<Parcel> parcels = new();
            Parcel p = DataSource.parcels.Find(parcel => parcel.CodeParcel == idP);
            if (p.CodeParcel == idP)
                throw new DoesntExistException("parcel does not exist");
            p.PickedUp = DateTime.Now;

            //    for (int i = 0; i < DataSource.parcels.Count; i++)
            //    {
            //        if (DataSource.parcels[i].CodeParcel == idP)
            //        {
            //            Parcel p = new Parcel();
            //            p.CodeParcel = DataSource.parcels[i].CodeParcel;
            //            p.Delivered = DataSource.parcels[i].Delivered;
            //            p.DroneId = DataSource.parcels[i].DroneId;
            //            p.PickedUp = DataSource.parcels[i].PickedUp;
            //            p.Priority = DataSource.parcels[i].Priority;
            //            p.Requested = DateTime.Now;
            //            p.Scheduled = DataSource.parcels[i].Scheduled;
            //            p.SenderId = DataSource.parcels[i].SenderId;
            //            p.TargetId = DataSource.parcels[i].TargetId;
            //            p.Weight = DataSource.parcels[i].Weight;
            //            DataSource.parcels[i] = p;
            //            break;
            //        }
            //    }

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
        //for (int i = 0; i < DataSource.parcels.Count; i++)
        //{
        //    if (DataSource.parcels[i].CodeParcel == idP)
        //    {

        //        Parcel p = new Parcel();
        //        p.CodeParcel = DataSource.parcels[i].CodeParcel;
        //        p.Delivered = DateTime.Now;
        //        p.DroneId = DataSource.parcels[i].DroneId;
        //        p.PickedUp = DataSource.parcels[i].PickedUp;
        //        p.Priority = DataSource.parcels[i].Priority;
        //        p.Requested = DataSource.parcels[i].Requested;
        //        p.Scheduled = DataSource.parcels[i].Scheduled;
        //        p.SenderId = DataSource.parcels[i].SenderId;
        //        p.TargetId = DataSource.parcels[i].TargetId;
        //        p.Weight = DataSource.parcels[i].Weight;
        //        DataSource.parcels[i] = p;

        //    }
        //}
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
            //DroneCharge dc = new DroneCharge();//לדעתי צריך כאן דווקא לשחרר את הישות של הרחפן שבטעינה ולא ליצור חדש
            //dc.DroneID = idD;
            //dc.StationID = idS;
        }

        /// <summary>
        /// the function searches the parcels that have not drone
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Parcel> ListParcelWithoutDrone()
        {
            List<Parcel> lstParcelWithoutDrone = DataSource.parcels.FindAll(parcel => parcel.DroneId <= 0);
            //List<Parcel> lstParcelWithoutDrone = new List<Parcel>();
            //for (int i = 0; i < DataSource.parcels.Count; i++)
            //{
            //    //
            //    if (DataSource.parcels[i].DroneId < 0)//למה גדול מאפס ולא קטן שווה?
            //    {
            //        lstParcelWithoutDrone.Add(DataSource.parcels[i]);
            //    }
            //}
            return lstParcelWithoutDrone;//האם זה בסדר שיתכן ויחזיר רשימה ריקה, או שיש לבדוק זאת ולזרוק חריגה אם כן?כנ"ל לגבי כל המתודות לעיל המחזירות רשימות
        }
        /// <summary>
        /// the function searches for the stations that have free slots of charge and creates a new list with them
        /// </summary>
        /// <returns>the new list of stations with free charge slots</returns>
        public IEnumerable<BaseStation> ListBaseStationsSlots()
        {
            List<BaseStation> lstStationsWithSlots = DataSource.stations.FindAll(station => station.ChargeSlots > 0);
            //List<BaseStation> lstSlots = new List<BaseStation>();
            //for (int i = 0; i < DataSource.stations.Count; i++)
            //{
            //    if (DataSource.stations[i].ChargeSlots > 0)
            //    {
            //        lstSlots.Add(DataSource.stations[i]);
            //    }
            //}
            return lstStationsWithSlots;
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
