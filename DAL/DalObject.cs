using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.IDAL.DO;
namespace DAL
{
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
                DataSource.stations.Add(b);
            }
            /// <summary>
            /// the function gets an object of drone and adds it to the list of drones
            /// </summary>
            /// <param name="d"></param>
            public static void AddDrone(Drone d)
            {
                DataSource.drones.Add(d);
            }
            /// <summary>
            /// the function gets an object of customer and adds it to the list of customers
            /// </summary>
            /// <param name="c"></param>
            public static void AddCustomer(Customer c)
            {
                DataSource.customers.Add(c);
            }
            /// <summary> 
            /// the function gets an object of parcel and adds it to the list of parcels
            /// </summary>
            /// <param name="p"></param>
               public static void AddParcel(Parcel p)
            {
                DataSource.parcels.Add(p);
            }
            /// <summary>
            /// the function gets 2 IDnumbers of parcel and drone, looks for this parcel in the list 
            /// and updates its drone field to be the drone the function got 
            /// </summary>
            /// <param name="idP"></param>
            /// <param name="idD"></param>
            public static void UpdateParcelToDrone(int idP, int idD)
            {
                for (int i = 0; i < DataSource.parcels.Count; i++)
                {
                    if (DataSource.parcels[i].CodeParcel==idP)
                    {
                        Parcel p = new Parcel();
                        p.CodeParcel = DataSource.parcels[i].CodeParcel;
                        p.Delivered = DataSource.parcels[i].Delivered;
                        p.DroneId = idD;
                        p.PickedUp = DataSource.parcels[i].PickedUp;
                        p.Priority = DataSource.parcels[i].Priority;
                        p.Requested = DateTime.Now;
                        p.Scheduled = DataSource.parcels[i].Scheduled;
                        p.SenderId = DataSource.parcels[i].SenderId;
                        p.TargetId = DataSource.parcels[i].TargetId;
                        p.Weight = DataSource.parcels[i].Weight;
                        DataSource.parcels[i] = p;
                        break;
                    }
                }    
            }
            public static void UpdatePickedUp(int idP)
            {
                for (int i = 0; i < DataSource.parcels.Count; i++)
                {
                    if (DataSource.parcels[i].CodeParcel == idP)
                    {
                        Parcel p = new Parcel();
                        p.CodeParcel = DataSource.parcels[i].CodeParcel;
                        p.Delivered = DataSource.parcels[i].Delivered;
                        p.DroneId = DataSource.parcels[i].DroneId;
                        p.PickedUp = DataSource.parcels[i].PickedUp;
                        p.Priority = DataSource.parcels[i].Priority;
                        p.Requested = DateTime.Now;
                        p.Scheduled = DataSource.parcels[i].Scheduled;
                        p.SenderId = DataSource.parcels[i].SenderId;
                        p.TargetId = DataSource.parcels[i].TargetId;
                        p.Weight = DataSource.parcels[i].Weight;
                        DataSource.parcels[i] = p;
                        break;
                    }
                }
            
            }
            public static void UpdateDeliver(int idP)
            {
                for (int i = 0; i < DataSource.parcels.Count; i++)
                {
                    if (DataSource.parcels[i].CodeParcel == idP)
                    {

                        Parcel p = new Parcel();
                        p.CodeParcel = DataSource.parcels[i].CodeParcel;
                        p.Delivered = DateTime.Now;
                        p.DroneId = DataSource.parcels[i].DroneId;
                        p.PickedUp = DataSource.parcels[i].PickedUp;
                        p.Priority = DataSource.parcels[i].Priority;
                        p.Requested = DataSource.parcels[i].Requested;
                        p.Scheduled = DataSource.parcels[i].Scheduled;
                        p.SenderId = DataSource.parcels[i].SenderId;
                        p.TargetId = DataSource.parcels[i].TargetId;
                        p.Weight = DataSource.parcels[i].Weight;
                        DataSource.parcels[i] = p;
                        
                    }
                }

            }
            public static void UpDateCharge(int idD , int idS)
            {
                for (int i = 0; i < DataSource.stations.Count; i++)
                {
                    if (DataSource.drones[i].CodeDrone == idD)
                    {
                        Drone d= new Drone();
                        d.CodeDrone = DataSource.drones[i].CodeDrone;
                        d.MaxWeight = DataSource.drones[i].MaxWeight;
                        d.ModelDrone = DataSource.drones[i].ModelDrone;
                        d.Status = DroneStatuses.maintenace;
                        d.Battery = DataSource.drones[i].Battery;
                        DataSource.drones[i] = d;
                        DroneCharge dc = new DroneCharge();//מה עושים עם המופע של הישות הזו? אולי מכניסים לרשימה?
                        dc.DroneID = idD;//אמורים להפחית באחד את מספר עמדות הטעינה הפנויות בתחנה שהתקבלה
                        dc.StationID = idS;

                    }
                }
            }
            public static void ReleaseCharge(int idD, int idS)
            {
                for (int i = 0; i < DataSource.stations.Count; i++)
                {
                    if (DataSource.drones[i].CodeDrone == idD)
                    {
                        Drone d = new Drone();
                        d.CodeDrone = DataSource.drones[i].CodeDrone;
                        d.MaxWeight = DataSource.drones[i].MaxWeight;
                        d.ModelDrone = DataSource.drones[i].ModelDrone;
                        d.Status = DroneStatuses.free;
                        d.Battery = DataSource.drones[i].Battery;
                        DataSource.drones[i] = d;
                        DroneCharge dc = new DroneCharge();//כאן אמורים לשחרר את עמדת הטעינה
                        dc.DroneID = idD;//אמורים להעלות באחד את מספר עמדות הטעינה הפנויות בתחנה שהתקבלה
                        dc.StationID = idS;

                    }
                }
            }
            public static BaseStation ShowStation(int idS)
            {
                for (int i = 0; i < DataSource.stations.Count; i++)
                {
                    if (DataSource.stations[i].CodeStation == idS)
                    {
                        return DataSource.stations[i];
                    }
                }
                BaseStation b =new BaseStation();
                return b;
            }
            public static Drone ShowDrone(int idD)
            {
                for (int i = 0; i < DataSource.drones.Count; i++)
                {
                    if (DataSource.drones[i].CodeDrone == idD)
                    {
                        return DataSource.drones[i];
                    }
                }
                Drone d = new Drone();
                return d;
            }
            public static Customer ShowCustomer(int idC)
            {
                for (int i = 0; i < DataSource.customers.Count; i++)
                {
                    if (DataSource.customers[i].IdCustomer == idC)
                    {
                        return DataSource.customers[i];
                    }
                }
                Customer c=new Customer();
                return c;
            }
            public static Parcel ShowParcel(int idP)
            {
                for (int i = 0; i < DataSource.parcels.Count; i++)
                {
                    if (DataSource.parcels[i].CodeParcel == idP)
                    {
                        return DataSource.parcels[i];
                    }
                }
                Parcel p=new Parcel();
                return p;
            }
            public static List<BaseStation> ShowListBaseStations()
            {
                return DataSource.stations;
            }
            public static List<Drone> ShowListDrones()
            {
                return DataSource.drones;
            }
            public static List<Customer> ShowListCustomers()
            {
                return DataSource.customers;
            }
            public static List<Parcel> ShowListParcels()
            {
                return DataSource.parcels;
            }
            public static List<Parcel> ListParcelWithoutDrone()
            {
                List<Parcel> lstParcelWithoutDrone = new List<Parcel>();
                for (int i = 0; i < DataSource.parcels.Count; i++)
                {
                    //
                    if (DataSource.parcels[i].DroneId > 0)//למה גדול מאפס ולא קטן שווה?
                    {
                        lstParcelWithoutDrone.Add(DataSource.parcels[i]);
                    }
                }
                return lstParcelWithoutDrone;
            }
            public static List<BaseStation> ListBaseStationsSlots()
            {
                List<BaseStation> lstSlots = new List<BaseStation>();
                for (int i = 0; i < DataSource.stations.Count; i++)
                {
                    if (DataSource.stations[i].ChargeSlots > 0)
                    {
                        lstSlots.Add(DataSource.stations[i]);
                    }
                }
                return lstSlots;
            }
        }
    }
}
