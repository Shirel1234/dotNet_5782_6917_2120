using DAL.IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    namespace DalObject
    {
       public class DataSource
        {
            internal static Random r = new Random();
            internal static List<BaseStation> stations = new List<BaseStation>();
            internal static List<Customer> customers = new List<Customer>();
            public static List<Drone> drones = new List<Drone>();
            internal static List<Parcel> parcels = new List<Parcel>();
            internal class Config
            {
                public static int countIdParcel = 1;
            }
            public static void Initialize()
            {
                BaseStation b = new BaseStation();
                for (int i = 1; i < 3; i++)
                {
                    b.CodeStation = i;
                    b.NameStation = r.Next(1000, 10000);
                    b.Longitude = r.Next(-180, 180);
                    b.Latitude = r.Next(-90, 90);
                    stations.Add(b);
                }
                Drone d = new Drone();
                for (int i = 1; i < 6; i++)
                {
                    d.CodeDrone = i;
                    d.ModelDrone = "model no."+i+"";
                    d.Status = (DroneStatuses)r.Next(0, 3);
                    d.MaxWeight= (WeightCategories)r.Next(0, 3);
                    d.Battery = (double)r.Next(0, 101);
                    drones.Add(d);
                }
                Customer c = new Customer();
                for (int i = 1; i < 11; i++)
                {
                    c.IdCustomer = r.Next(100000000, 1000000000);
                    c.NameCustomer = (Names)r.Next(0, 10);
                    c.Phone = "050-" + r.Next(1000000, 10000000)+"";
                    c.Longitude = r.Next(-180, 180);
                    c.Latitude = r.Next(-90, 90);
                    customers.Add(c);
                }
                Parcel p = new Parcel();
                for (int i = 1; i < 11; i++)
                {
                    p.CodeParcel = i;
                    p.SenderId = r.Next(100000000, 1000000000);
                    p.TargetId = r.Next(100000000, 1000000000);
                    p.Weight = (WeightCategories)r.Next(0, 3);
                    p.Priority = (Priorities)r.Next(0, 3);
                    p.DroneId = 0;
                    p.Requested =new DateTime(2021,10,i); 
                    p.Scheduled= new DateTime(2021, 10, 1+i);
                    p.PickedUp = new DateTime(2021, 10, 2+i);
                    p.Delivered= new DateTime(2021, 10, 3+i);
                    parcels.Add(p);
                }
            }
        }
    }

}