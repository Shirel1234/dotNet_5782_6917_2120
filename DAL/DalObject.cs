using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

  namespace DalObject
    {
      public class DalObject
        {
            /// <summary>
            /// constructor of the class DalObject: it calls the function of initializing from DataSource.
            /// </summary>
            public DalObject()
            {
                DataSource.Initialize();
            }
            /// <summary>
            /// the function gets an object of base station and adds it to the list of base stations
            /// </summary>
            /// <param name="b"></param>
            public static void AddStation(BaseStation b)
            {
                List<BaseStation> baseStations = new();
                BaseStation temp = DataSource.stations.FirstOrDefault(station => station.CodeStation == b.CodeStation);
                if(temp.CodeStation==b.CodeStation)
                    throw new Exceptions.AddingExceptions(b.CodeStation, "base station already exists");
                DataSource.stations.Add(b); 
            }
            /// <summary>
            /// the function gets an object of drone and adds it to the list of drones
            /// </summary>
            /// <param name="d"> a drone</param>
            public static void AddDrone(Drone d)
            {
            Drone temp = DataSource.drones.FirstOrDefault(drone => drone.CodeDrone == d.CodeDrone);
            if(temp.CodeDrone==d.CodeDrone)
                    throw new Exceptions.AddingExceptions(d.CodeDrone, "drone already exists");
                DataSource.drones.Add(d);
            }
            /// <summary>
            /// the function gets an object of customer and adds it to the list of customers
            /// </summary>
            /// <param name="c"> a customer</param>
            public static void AddCustomer(Customer c)
            {
            Customer temp = DataSource.customers.FirstOrDefault(customer => customer.IdCustomer == c.IdCustomer);
            if(temp.IdCustomer==c.IdCustomer)
                    throw new Exceptions.AddingExceptions(c.IdCustomer, "customer already exists");
                DataSource.customers.Add(c);
            }
            /// <summary> 
            /// the function gets an object of parcel and adds it to the list of parcels
            /// </summary>
            /// <param name="p"> a parcel</param>
            public static void AddParcel(Parcel p)
            {
                Parcel temp = DataSource.parcels.FirstOrDefault(parcel => parcel.CodeParcel == p.CodeParcel);
                if(temp.CodeParcel==p.CodeParcel)
                    throw new Exceptions.AddingExceptions(p.CodeParcel, "parcel already exists");
                DataSource.parcels.Add(p);
            }
            /// <summary>
            /// the function looks for this parcel in the list 
            /// and updates its drone field to be the drone the function got 
            /// </summary>
            /// <param name="idP"> an id of parcel</param>
            /// <param name="idD">an id of drone</param>
            
            public static void UpdateParcelToDrone(int idP, int idD)
            {
                List<Parcel> parcels = new();
                Parcel p = DataSource.parcels.Find(parcel => parcel.CodeParcel == idP);
                if(p.CodeParcel==idP)
                    throw new Exceptions.UpdateExceptions(idP, "parcel does not exist");
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
            public static void UpdatePickedUp(int idP)
            {
                List<Parcel> parcels = new();
                Parcel p = DataSource.parcels.Find(parcel => parcel.CodeParcel == idP);
                if (p.CodeParcel==idP)
                    throw new Exceptions.UpdateExceptions(idP, "parcel does not exist");
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
            public static void UpdateDeliver(int idP)
            {
                //List<Parcel> parcels = new();
                Parcel p = DataSource.parcels.Find(parcel => parcel.CodeParcel == idP);
                if (p.CodeParcel==idP)
                    throw new Exceptions.UpdateExceptions(idP, "parcel does not exist");
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
            public static void UpDateCharge(int idD , int idS)
            {
                Drone d = DataSource.drones.Find(drone => drone.CodeDrone == idD);
                if (d.CodeDrone== idD)
                    throw new Exceptions.UpdateExceptions(idD, "drone does not exist"); 
                DroneCharge dc = new DroneCharge();
                dc.DroneID = idD;
                dc.StationID = idS;
            }
            ///<summary>
            /// the function searches the drone with this id and updates its status to be free   
            /// </summary>
            /// <param name="idD"> an id of drone</param>
            /// <param name="idS">an id of station</param>
            public static void ReleaseCharge(int idD, int idS)
            {
                Drone d = DataSource.drones.Find(drone => drone.CodeDrone == idD);
                if (d.CodeDrone ==idD)
                    throw new Exceptions.UpdateExceptions(idD, "drone does not exist");
                //DroneCharge dc = new DroneCharge();//לדעתי צריך כאן דווקא לשחרר את הישות של הרחפן שבטעינה ולא ליצור חדש
                //dc.DroneID = idD;
                //dc.StationID = idS;
            }
            
            /// <summary>
            /// the function searches the station with the id that it got
            /// </summary>
            /// <param name="idS"></param>
            /// <returns>the details of this station</returns>
            public static BaseStation ShowStation(int idS)
            {
               BaseStation b = DataSource.stations.Find(station => station.CodeStation == idS);
                if (b.CodeStation == idS)
                    throw new Exceptions.UpdateExceptions(idS, "base station does not exist");
                return b;
            }
            /// <summary>
            /// the function searches the drone with the id that it got
            /// </summary>
            /// <param name="idD"></param>
            /// <returns>the details of this drone</returns>
            public static Drone ShowDrone(int idD)
            {
                Drone d = DataSource.drones.Find(drone => drone.CodeDrone == idD);
                if (d.CodeDrone == idD)
                    throw new Exceptions.UpdateExceptions(idD, "drone does not exist");
                return d;
            }
            /// <summary>
            /// the function searchs the customer with the id that it got
            /// </summary>
            /// <param name="idC"></param>
            /// <returns>the details of this customer</returns>
            public static Customer ShowCustomer(int idC)
            {
                Customer c = DataSource.customers.Find(customer => customer.IdCustomer == idC);
                if (c.IdCustomer == idC)
                    throw new Exceptions.UpdateExceptions(idC, "customer does not exist");
                return c;
            }
            /// <summary>
            /// the function searchs the parcel with the id that it got
            /// </summary>
            /// <param name="idP"></param>
            /// <returns>the details of this parcel</returns>
            public static Parcel ShowParcel(int idP)
            {
                Parcel p = DataSource.parcels.Find(parcel => parcel.CodeParcel == idP);
                if (p.CodeParcel == idP)
                    throw new Exceptions.UpdateExceptions(idP, "parcel does not exist");
                return p;
            }
            /// <summary>
            /// the function show the list of stations
            /// </summary>
            /// <returns>list of stations</returns>
            public static IEnumerable<BaseStation> ShowListBaseStations()
            {
                return DataSource.stations;
            }
            /// <summary>
            /// the function show the list of drones
            /// </summary>
            /// <returns>list of drones</returns>
            public static IEnumerable<Drone> ShowListDrones()
            {
                return DataSource.drones;
            }
            /// <summary>
            /// the function show the list of customers
            /// </summary>
            /// <returns>list of customers</returns>
            public static IEnumerable<Customer> ShowListCustomers()
            {
                return DataSource.customers;
            }
            /// <summary>
            /// the function show the list of parcels
            /// </summary>
            /// <returns>list of parcels</returns>
            public static IEnumerable<Parcel> ShowListParcels()
            {
                return DataSource.parcels;
            }
            /// <summary>
            /// the function searches the parcels that have not drone
            /// </summary>
            /// <returns></returns>
            public static IEnumerable<Parcel> ListParcelWithoutDrone()
            {
                List<Parcel> lstParcelWithoutDrone = DataSource.parcels.FindAll(parcel => parcel.DroneId <=0);
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
            public static IEnumerable<BaseStation> ListBaseStationsSlots()
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
            public static double[] AskElectrical(Drone d)
            {
            double[] arrElectrical = new double[5];
            arrElectrical[0] =DataSource.Config.free;
            arrElectrical[1] = DataSource.Config.easy;
            arrElectrical[2] = DataSource.Config.medium;
            arrElectrical[3] = DataSource.Config.heavy;
            arrElectrical[4] = DataSource.Config.chargingRate;
            return arrElectrical;
            }
      }
  }
