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
           public DalObject()
            {
                DataSource.Initialize();
            }
            public void AddStation(BaseStation b)
            {
                DataSource.stations.Add(b);
            }
            public void AddDrone(Drone d)
            {
                DataSource.drones.Add(d);
            }
            public void AddCustomer(Customer c)
            {
                DataSource.customers.Add(c);
            }
            public void AddParcel(Parcel p)
            {
                DataSource.parcels.Add(p);
            }
            public void UpdateParcelToDrone(int idP, int idD)
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
            public void UpdatePickedUp(int idP)
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
            public void UpdateDeliver(int idP)
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
            public void UpDateCharge(int idD , int idS)
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
                        DroneCharge dc = new DroneCharge();
                        dc.DroneID = idD;
                        dc.StationID = idS;

                    }
                }
            }
            public void ReleaseCharge(int idD, int idS)
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
                        DroneCharge dc = new DroneCharge();
                        dc.DroneID = idD;
                        dc.StationID = idS;

                    }
                }
            }
            public BaseStation ShowStation(int idS)
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
            public Drone ShowDrone(int idD)
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
            public Customer ShowCustomer(int idC)
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
            public Parcel ShowParcel(int idP)
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
            public List<BaseStation> ShowListBaseStations()
            {
                return DataSource.stations;
            }
            public List<Drone> ShowListDrones()
            {
                return DataSource.drones;
            }
            public List<Customer> ShowListBaseCustomer()
            {
                return DataSource.customers;
            }
            public List<Parcel> ShowListBaseParcel()
            {
                return DataSource.parcels;
            }
            public List<Parcel> ListParcelWithoutDrone()
            {
                List<Parcel> lstParcelWithoutDrone = new List<Parcel>();
                for (int i = 0; i < DataSource.parcels.Count; i++)
                {
                    //
                    if (DataSource.parcels[i].DroneId > 0)
                    {
                        lstParcelWithoutDrone.Add(DataSource.parcels[i]);
                    }
                }
                return lstParcelWithoutDrone;
            }
            public List<BaseStation> ListBaseStationsSlots()
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
