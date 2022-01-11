﻿using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dal
{
    class DataSourceXml
    {
        internal static Random r = new Random();
        internal static List<BaseStation> stations = new List<BaseStation>();
        internal static List<Customer> customers = new List<Customer>();
        internal static List<Drone> drones = new List<Drone>();
        internal static List<Parcel> parcels = new List<Parcel>();
        internal static List<DroneCharge> dronesCharge = new List<DroneCharge>();
        internal class Config
        {
            internal static int countIdParcel = 200;
            internal static double Free { get => 0.01; }
            internal static double Easy { get => 0.011; }
            internal static double Medium { get => 0.012; }
            internal static double Heavy { get => 0.013; }
            internal static double ChargingRate { get => 10.25; }
        }
        public static void Initialize()
        {
            BaseStation b = new BaseStation();
            //XElement baseStationRoot = new XElement("Station", 0, 0, 0, 0);
            XElement baseStationRoot = XMLTools.LoadListFromXMLElement(@"StationsXml.xml");
            for (int i = 1; i < 3; i++)
            {
                b.CodeStation = i;
                b.NameStation = r.Next(1000, 10000);
                b.Longitude = r.NextDouble() + r.Next(30, 34);
                b.Latitude = r.NextDouble() + r.Next(34, 37);
                b.ChargeSlots = r.Next(10, 50);
                stations.Add(b);
                XElement id = new XElement("id", i);
                XElement name = new XElement("name", b.NameStation);
                XElement numOfChargeSlots = new XElement("numOfChargeSlots", b.ChargeSlots);
                XElement longitude = new XElement("longitude", b.Longitude);
                XElement latitude = new XElement("latitude", b.Latitude);
                XElement location = new XElement("location", longitude, latitude);
                baseStationRoot.Add(new XElement("Station", id, name, numOfChargeSlots, location));
                XMLTools.SaveListToXMLElement(baseStationRoot, @"StationsXml.xml");

            }

            Drone d = new Drone();
            for (int i = 1; i < 6; i++)
            {
                d.CodeDrone = i;
                d.ModelDrone = "model no." + i + "";
                d.MaxWeight = (WeightCategories)r.Next(0, 3);
                drones.Add(d);
            }
            XMLTools.SaveListToXMLSerializer(drones, @"DronesXml.xml");
            dronesCharge.Add(new DroneCharge() { DroneID = 1, StationID = 1111, BeginingCharge = DateTime.Now });
            XMLTools.SaveListToXMLSerializer(dronesCharge, @"DronesChargeXml.xml");

            Customer c = new Customer();
            for (int i = 1; i < 11; i++)
            {
                c.IdCustomer = r.Next(100000000, 1000000000);
                c.NameCustomer = "customer" + i;
                c.Phone = "050-" + r.Next(1000000, 10000000) + "";
                c.Longitude = r.NextDouble() + r.Next(30, 34);
                c.Latitude = r.NextDouble() + r.Next(34, 37);
                customers.Add(c);
            }
            XMLTools.SaveListToXMLSerializer(customers, @"CustomersXml.xml");
            Parcel p = new Parcel();
            int[] arrTemp = new int[5] { 0, 0, 0, 0, 0 };
            for (int i = 0, j = 9, m = 0; i < 10; i++, j--, m++)
            {
                p.CodeParcel = Config.countIdParcel++;
                p.SenderId = customers[i].IdCustomer;
                p.TargetId = customers[j].IdCustomer;
                p.Weight = (WeightCategories)r.Next(0, 3);
                p.Priority = (Priorities)r.Next(0, 3);
                p.Requested = DateTime.Now;
                int numDrone = m;
                var listDrones = from droneMatchW in drones where droneMatchW.MaxWeight >= p.Weight select droneMatchW;
                var drone = listDrones.FirstOrDefault(d => arrTemp[d.CodeDrone - 1] == 0);
                p.DroneId = drone.CodeDrone;
                if (p.DroneId != 0)
                {
                    arrTemp[p.DroneId - 1] = 1;
                    p.Scheduled = DateTime.Now;
                    p.PickedUp = new DateTime(0);
                    p.Delivered = new DateTime(0);
                }
                else
                {
                    p.Scheduled = new DateTime(0);
                    p.PickedUp = new DateTime(0);
                    p.Delivered = new DateTime(0);
                }
                parcels.Add(p);
            }
            XMLTools.SaveListToXMLSerializer(parcels, @"ParcelsXml.xml");

        }
    }
}