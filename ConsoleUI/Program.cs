﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.IDAL.DO;


namespace ConsoleUI
{
    class Program
    {
        public static void addingOptions()
        {
            Console.WriteLine("Choose:\n1:add base station\n2:add drone\n3:add customer\n4:add parcel\n");
            int ch = int.Parse(Console.ReadLine());
            switch (ch)
            {
                case 1: newBaseStation(); break;
                case 2: newDrone(); break;
                case 3: newCustomer(); break;
                case 4: newParcel(); break;
                default: Console.WriteLine("error\n"); break;
            }
        }
        public static void newBaseStation()
        {
            Console.WriteLine("enter ID number, name, longitude, lattitude and number of charge slots of the base station\n");
            int id = int.Parse(Console.ReadLine());
            int name = int.Parse(Console.ReadLine());
            int chargeSlots = int.Parse(Console.ReadLine());
            double longitude = double.Parse(Console.ReadLine());
            double lattitude = double.Parse(Console.ReadLine());
            DAL.IDAL.DO.BaseStation baseStation = new DAL.IDAL.DO.BaseStation();
            baseStation.CodeStation = id;
            baseStation.NameStation = name;
            baseStation.ChargeSlots = chargeSlots;
            baseStation.Longitude = longitude;
            baseStation.Latitude = lattitude;
            DAL.DalObject.DalObject.AddStation(baseStation);//זה עובד רק אם מוסיפים סטטיק לפני המתודות בדל אובג'קט, לבדוק שזה בסדר
        }
        public static void newDrone()
        {
            Console.WriteLine("enter ID number, model, battery status," +
                " and max weight(easy, medium, heavy) of the drone\n");
            int id = int.Parse(Console.ReadLine());
            string model = Console.ReadLine();
            int battery = int.Parse(Console.ReadLine());//האם לבדוק תקינות קלט?
            WeightCategories weight = (WeightCategories)Console.Read();
            DAL.IDAL.DO.Drone drone = new DAL.IDAL.DO.Drone();
            drone.CodeDrone = id;
            drone.ModelDrone = model;
            drone.Battery = battery;
            drone.Status = DroneStatuses.free;
            drone.MaxWeight = weight;
            DAL.DalObject.DalObject.AddDrone(drone);
        }
        public static void newCustomer()
        {
            Console.WriteLine("enter ID number, name, phone number," +
                " longitude and lattitude of the customer\n");
            int id = int.Parse(Console.ReadLine());
            string name = Console.ReadLine();
            string phone = Console.ReadLine();
            double longitude = double.Parse(Console.ReadLine());
            double lattitude = double.Parse(Console.ReadLine());
            DAL.IDAL.DO.Customer customer = new DAL.IDAL.DO.Customer();
            customer.IdCustomer = id;
            customer.NameCustomer = name;//לסדר את עניין ההגרלה
            customer.Phone = phone;
            customer.Longitude = longitude;
            customer.Latitude = lattitude;
            DAL.DalObject.DalObject.AddCustomer(customer);
        }
        public static void newParcel()
        {
            Console.WriteLine("enter parcel ID number, sender ID number, target customer ID number," +
                " weight(easy, medium, heavy), priority (normal, express, emergency) //and time of creating the parcel for sending//\n");
            int idP = int.Parse(Console.ReadLine());
            int idS = int.Parse(Console.ReadLine());
            int idT = int.Parse(Console.ReadLine());
            WeightCategories weight = (WeightCategories)Console.Read();
            Priorities priority = (Priorities)Console.Read();
            DateTime creatingTime = DateTime.Now;
            DAL.IDAL.DO.Parcel parcel = new DAL.IDAL.DO.Parcel();
            parcel.CodeParcel = idP;
            parcel.SenderId = idS;
            parcel.TargetId = idT;
            parcel.Weight = weight;
            parcel.Priority = priority;
            DAL.DalObject.DalObject.AddParcel(parcel);
        }
        public static void updateOptions()
        {
            Console.WriteLine("Choose:\n1:update matching parcel to drone\n2:update picking up parcel by drone\n" +
               "3:update delevered parcel to customer\n4:update sending of drone to charge\n5:update releaseing of drone from charging\n");
            int ch = int.Parse(Console.ReadLine());
            switch (ch)
            {
                case 1: parcelToDroneUpdate(); break;
                case 2: pickedUpUpdate(); break;
                case 3: deliveredUpdate(); break;
                case 4: chargeUpdate(); break;
                case 5: releaseFromChargeUpdate(); break;
                default: Console.WriteLine("error\n"); break;
            }
        }//
        public static void parcelToDroneUpdate()//אנחנו אמורות לשייך את הרחפן לחבילה או רק לעדכן את השיוך שכבר נעשה?
        {
            Console.WriteLine("enter the parcel ID and the drone ID\n");
            int idP = int.Parse(Console.ReadLine());
            int idD = int.Parse(Console.ReadLine());
            DAL.DalObject.DalObject.UpdateParcelToDrone(idP, idD);
        }
        public static void pickedUpUpdate()
        {
            Console.WriteLine("enter the parcel ID\n");
            int idP = int.Parse(Console.ReadLine());
            DAL.DalObject.DalObject.UpdatePickedUp(idP);
        }
        public static void deliveredUpdate()
        {
            Console.WriteLine("enter the parcel ID\n");
            int idP = int.Parse(Console.ReadLine());
            DAL.DalObject.DalObject.UpdateDeliver(idP);
        }
        public static void chargeUpdate()
        {
            Console.WriteLine("enter the drone ID\n");
            int idD = int.Parse(Console.ReadLine());
            Console.WriteLine("Choose one of the following base stations for charging. Enter its ID number\n");
            baseStationsWithChargeSlots();
            int idS = int.Parse(Console.ReadLine());
            DAL.DalObject.DalObject.UpDateCharge(idD, idS);
        }

        public static void releaseFromChargeUpdate()
        {
            Console.WriteLine("enter the drone ID and the base station ID\n");
            int idD = int.Parse(Console.ReadLine());
            int idS = int.Parse(Console.ReadLine());
            DAL.DalObject.DalObject.ReleaseCharge(idD, idS);
        }
        public static void viewOptions()
        {
            Console.WriteLine("Choose:\n1:base station view\n2:drone view\n3:customer view\n4:parcel view\n");
            int ch = int.Parse(Console.ReadLine());
            switch (ch)
            {
                case 1: baseStationView(); break;
                case 2: droneView(); break;
                case 3: customerView(); break;
                case 4: parcelView(); break;
                default: Console.WriteLine("error\n"); break;
            }
        }

        public static void baseStationView()
        {
            Console.WriteLine("enter the base station ID\n");
            int idS = int.Parse(Console.ReadLine());
            DAL.IDAL.DO.BaseStation baseStation = DAL.DalObject.DalObject.ShowStation(idS);
            if (baseStation.CodeStation==idS)//איך לבדוק שהאובייקט לא ריק
                Console.WriteLine(baseStation + "\n");
            else
                Console.WriteLine("Sorry. The base station is not found\n");
        }
        public static void droneView()
        {
            Console.WriteLine("enter the drone ID\n");
            int idD = int.Parse(Console.ReadLine());
            DAL.IDAL.DO.Drone drone = DAL.DalObject.DalObject.ShowDrone(idD);
            if (drone.CodeDrone==idD)//איך לבדוק שהאובייקט לא ריק
                Console.WriteLine(drone + "\n");
            else
                Console.WriteLine("Sorry. The drone is not found\n");
        }
        public static void customerView()
        {
            Console.WriteLine("enter the customer ID\n");
            int idC = int.Parse(Console.ReadLine());
            DAL.IDAL.DO.Customer customer = DAL.DalObject.DalObject.ShowCustomer(idC);
            if (customer.IdCustomer==idC)//איך לבדוק שהאובייקט לא ריק
                Console.WriteLine(customer + "\n");
            else
                Console.WriteLine("Sorry. The customer is not found\n");
        }
        public static void parcelView()
        {
            Console.WriteLine("enter the parcel ID\n");
            int idP = int.Parse(Console.ReadLine());
            DAL.IDAL.DO.Parcel parcel = DAL.DalObject.DalObject.ShowParcel(idP);
            if (parcel.CodeParcel==idP)//איך לבדוק שהאובייקט לא ריק
                Console.WriteLine(parcel + "\n");
            else
                Console.WriteLine("Sorry. The parcel is not found\n");
        }
        public static void listViewOptions()
        {
            Console.WriteLine("Choose:\n1:a list of base stations view\n2:a list of drones view\n3:a list of customers view\n" +
                    "4:list of parcels view\n5:view of list of parcels not yet matched to a drone\n" +
                    "6:view of base stations with available charging stations\n");
            int ch = int.Parse(Console.ReadLine());
            switch (ch)
            {
                case 1: baseStationsListView(); break;
                case 2: dronesListView(); break;
                case 3: customersListView(); break;
                case 4: parcelsListView(); break;
                case 5: parcelsWithoutdrone(); break;
                case 6: baseStationsWithChargeSlots(); break;
                default: Console.WriteLine("error\n"); break;
            }
        }
        public static void baseStationsListView()
        {
            List<BaseStation> baseStations = DAL.DalObject.DalObject.ShowListBaseStations();
            for (int i = 1; i <= baseStations.Count; i++)
                Console.WriteLine("Base station No. " + i + ":\n" + baseStations[i]);
        }
        public static void dronesListView()
        {
            List<Drone> drones = DAL.DalObject.DalObject.ShowListDrones();
            for (int i = 1; i <= drones.Count; i++)
                Console.WriteLine("Drone No. " + i + ":\n" + drones[i]);
        }
        public static void customersListView()
        {
            List<Customer> customers = DAL.DalObject.DalObject.ShowListCustomers();
            for (int i = 1; i <= customers.Count; i++)
                Console.WriteLine("Customer No. " + i + ":\n" + customers[i]);
        }
        public static void parcelsListView()
        {
            List<Parcel> parcels = DAL.DalObject.DalObject.ShowListParcels();
            for (int i = 1; i <= parcels.Count; i++)
                Console.WriteLine("Parcel No. " + i + ":\n" + parcels[i]);
        }
        public static void parcelsWithoutdrone()
        {
            List<Parcel> parcels = DAL.DalObject.DalObject.ListParcelWithoutDrone();
            for (int i = 1; i <= parcels.Count; i++)
                Console.WriteLine("Parcel No. " + i + ":\n" + parcels[i]);
        }
        public static void baseStationsWithChargeSlots()
        {
            List<BaseStation> baseStations = DAL.DalObject.DalObject.ListBaseStationsSlots();
            for (int i = 1; i <= baseStations.Count; i++)
                Console.WriteLine("Parcel No. " + i + ":\n" + baseStations[i]);
        }
        public static void Main(string[] args)
        {
            char choice;
            do
            {
                Console.WriteLine("Choose one of the following:\na:Options of adding\n" +
                    "u:Options of update\n" + "v:Options of view\nl:Options of list view\ne: Exit");
                choice = (Char)System.Console.Read();
                switch (choice)
                {
                    case 'a': addingOptions(); break;
                    case 'u': updateOptions(); break;
                    case 'v': viewOptions(); break;
                    case 'l': listViewOptions(); break;
                    case 'e': Console.WriteLine("bye\n"); break;
                    default: Console.WriteLine("error\n"); break;
                }
            }
            while (choice != 'e');
        }
         
    }
}















