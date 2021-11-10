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
                try
                {
                    List<BaseStation> baseStations = new();
                    for (int i = 0; i < baseStations.Count; i++)
                    {
                        // if (baseStations[i].CodeStation == b.CodeStation)
                        // thrownew ArgumentException( "Error. This base station is already exists");
                        DataSource.stations.Add(b);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            /// <summary>
            /// the function gets an object of drone and adds it to the list of drones
            /// </summary>
            /// <param name="d"> a drone</param>
            public static void AddDrone(Drone d)
            {
                DataSource.drones.Add(d);
            }
            /// <summary>
            /// the function gets an object of customer and adds it to the list of customers
            /// </summary>
            /// <param name="c"> a customer</param>
            public static void AddCustomer(Customer c)
            {
                DataSource.customers.Add(c);
            }
            /// <summary> 
            /// the function gets an object of parcel and adds it to the list of parcels
            /// </summary>
            /// <param name="p"> a parcel</param>
               public static void AddParcel(Parcel p)
            {
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
                for (int i = 0; i < parcels.Count; i++)
                {
                    if(parcels[i].CodeParcel==idP)
                    {
                        Parcel p = parcels[i];
                        p.DroneId = idD;
                        p.Requested = DateTime.Now;
                        parcels[i] = p;
                        break;
                    }   

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
            }
            /// <summary>
            /// the function searchs the parcel with this id  and updates the parcel to be pick up
            /// </summary>
            /// <param name="idP"> an id of parcel</param>
            public static void UpdatePickedUp(int idP)
            {
                List<Parcel> parcels = new();
                for (int i = 0; i < parcels.Count; i++)
                {
                  if(parcels[i].CodeParcel==idP)
                    {
                        Parcel p = parcels[i];
                        p.PickedUp = DateTime.Now;
                        parcels[i] = p;
                        break;
                    }
                }
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
                List<Parcel> parcels = new();
                for (int i = 0; i < parcels.Count; i++)
                {
                    if (parcels[i].CodeParcel == idP)
                    {
                        Parcel p = parcels[i];
                        p.Delivered = DateTime.Now;
                        parcels[i] = p;
                        break;
                    }
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

            }
            /// <summary>
            /// the function searches the drone with this id and updates its status to be maintenace
            /// in additions it creates a droneCharge and uptates its id of drone and id of station
            /// </summary>
            /// <param name="idD"> an id of drone</param>
            /// <param name="idS">an id of station</param>
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
                       // d.Status = DroneStatuses.maintenace;
                       // d.Battery = DataSource.drones[i].Battery;
                        DataSource.drones[i] = d;
                        DroneCharge dc = new DroneCharge();
                        dc.DroneID = idD;
                        dc.StationID = idS;

                    }
                }
            }
            /// <summary>
            /// the function searches the drone with this id and updates its status to be free   
            /// </summary>
            /// <param name="idD"> an id of drone</param>
            /// <param name="idS">an id of station</param>
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
                      //  d.Status = DroneStatuses.free;
                       // d.Battery = DataSource.drones[i].Battery;
                        DataSource.drones[i] = d;
                        DroneCharge dc = new DroneCharge();
                        dc.DroneID = idD;
                        dc.StationID = idS;

                    }
                }
            }
            /// <summary>
            /// the function searches the station with the id that it got
            /// </summary>
            /// <param name="idS"></param>
            /// <returns>the details of this station</returns>
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
            /// <summary>
            /// the function searches the drone with the id that it got
            /// </summary>
            /// <param name="idD"></param>
            /// <returns>the details of this drone</returns>
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
            /// <summary>
            /// the function searchs the customer with the id that it got
            /// </summary>
            /// <param name="idC"></param>
            /// <returns>the details of this customer</returns>
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
            /// <summary>
            /// the function searchs the parcel with the id that it got
            /// </summary>
            /// <param name="idP"></param>
            /// <returns>the details of this parcel</returns>
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
            /// <summary>
            /// the function searches tha stations thats have charges of slot are free and creates a new list with them
            /// </summary>
            /// <returns>the new list of stations with chargeslots free</returns>
            public static IEnumerable<BaseStation> ListBaseStationsSlots()
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
