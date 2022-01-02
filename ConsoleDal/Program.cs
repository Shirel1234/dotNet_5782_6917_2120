﻿//using System;

//namespace ConsoleDal
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            Console.WriteLine("Hello World!");
//        }
//    }
//}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DO;

namespace ConsoleDal
{
    class Program
    {
        static IDal dal = DalFactory.GetDal();
        enum Menu { Exit, Add, View, ViewList }
        enum AddOption { Exit, Station, Drone, Customer, Parcel }
        enum ShowOption { Exit, Station, Drone, Customer, Parcel }
        enum ShowListOption { Exit, Station, Drone, Customers, Parcels, UnAssignmentParcel, AvailableChargingsStation }
        /// <summary>
        /// the function offers the user 4 adding options for adding base station/drone/customer/parcel.
        /// </summary>
        public static void AddingOptions()
        {
            AddOption addOp;
            int choice;

            Console.WriteLine("Choose:\n1-add base station\n2-add drone\n3-add customer\n4-add parcel\n0-exit");
            if (int.TryParse(Console.ReadLine(), out choice))
            {
                addOp = (AddOption)choice;

                //Console.WriteLine("enter number:");
                //bool success = int.TryParse(Console.ReadLine(), out x);
                // Console.WriteLine(x);
                {
                    switch (addOp)
                    {
                        case AddOption.Station: NewBaseStation(); break;
                        case AddOption.Drone: newDrone(); break;
                        case AddOption.Customer: newCustomer(); break;
                        case AddOption.Parcel: newParcel(); break;
                        case AddOption.Exit: break;
                        default: Console.WriteLine("error\n"); break;
                    }
                }
            }
        }
        /// <summary>
        /// the function adds a new base station to the list.
        /// it creates a new object of base station, initializes its fields accordind to the input from the user and adds it to the list.
        /// </summary>
        public static void NewBaseStation()
        {
            Console.WriteLine("enter ID number, name, number of charging positions of the base station, longitude and lattitude");
            int id; int name; int chargeSlots;
            if (int.TryParse(Console.ReadLine(), out id))
                if (int.TryParse(Console.ReadLine(), out name))
                    if (int.TryParse(Console.ReadLine(), out chargeSlots))
                    {
                        int tempLon = int.Parse(Console.ReadLine());
                        int tempLat = int.Parse(Console.ReadLine());
                        double longitude = (double)tempLon;
                        double lattitude = (double)tempLat;
                        DO.BaseStation baseStation = new DO.BaseStation();
                        baseStation.CodeStation = id;
                        baseStation.NameStation = name;
                        baseStation.ChargeSlots = chargeSlots;
                        baseStation.Longitude = longitude;
                        baseStation.Latitude = lattitude;
                        dal.AddStation(baseStation);
                    }

        }
        /// <summary>
        /// the function adds a new drone to the list.
        /// it creates a new object of drone, initializes its fields accordind to the input from the user and adds it to the list.
        /// </summary>
        public static void newDrone()
        {
            Console.WriteLine("enter ID number, model, battery status," +
                " and max weight(easy, medium, heavy) of the drone\n");
            int id;
            if (int.TryParse(Console.ReadLine(), out id))
            {
                string model = Console.ReadLine();
                int tempBattery = int.Parse(Console.ReadLine());
                double battery = (double)tempBattery;
                WeightCategories weight = (WeightCategories)Console.Read();
                DO.Drone drone = new DO.Drone();
                drone.CodeDrone = id;
                drone.ModelDrone = model;
                //   drone.Battery = battery;
                //
                //drone.Status = DroneStatuses.free;
                drone.MaxWeight = weight;
                dal.AddDrone(drone);
            }
        }
        /// <summary>
        /// the function adds a new customer to the list.
        /// it creates a new object of customer, initializes its fields accordind to the input from the user and adds it to the list.
        /// </summary>
        public static void newCustomer()
        {
            Console.WriteLine("enter ID number, name, phone number,longitude and lattitude of the customer");
            int id;
            if (int.TryParse(Console.ReadLine(), out id))
            {
                string name = Console.ReadLine();
                string phone = Console.ReadLine();
                int tempLon = int.Parse(Console.ReadLine());
                int tempLat = int.Parse(Console.ReadLine());
                double longitude = (double)tempLon;
                double lattitude = (double)tempLat;
                DO.Customer customer = new DO.Customer();
                customer.IdCustomer = id;
                customer.NameCustomer = name;
                customer.Phone = phone;
                customer.Longitude = longitude;
                customer.Latitude = lattitude;
                dal.AddCustomer(customer);
            }
        }
        /// <summary>
        /// the function adds a new parcel to the list.
        /// it creates a new object of parcel, initializes its fields accordind to the input from the user and adds it to the list.
        /// </summary>
        public static void newParcel()
        {
            Console.WriteLine("enter parcel ID number, sender ID number, target customer ID number,weight(easy, medium, heavy) and priority (normal, express, emergency) of the parcel //and time of creating the parcel for sending//");
            int idP; int idS; int idT;
            if (int.TryParse(Console.ReadLine(), out idP))
                if (int.TryParse(Console.ReadLine(), out idS))
                    if (int.TryParse(Console.ReadLine(), out idT))
                    {
                        WeightCategories weight = (WeightCategories)Console.Read();
                        Priorities priority = (Priorities)Console.Read();
                        DateTime creatingTime = DateTime.Now;
                        DO.Parcel parcel = new DO.Parcel();
                        parcel.CodeParcel = idP;
                        parcel.SenderId = idS;
                        parcel.TargetId = idT;
                        parcel.Weight = weight;
                        parcel.Priority = priority;
                        dal.AddParcel(parcel);
                    }
        }

        /// <summary>
        /// the function offers the user 4 view options for view of a base station/a drone/a customer/a parcel
        /// </summary>
        public static void viewOptions()
        {
            Console.WriteLine("Choose:\n1:base station view\n2:drone view\n3:customer view\n4:parcel view\n0:Exit");
            int choice;
            ShowOption showOption;
            if (int.TryParse(Console.ReadLine(), out choice))
            {
                showOption = (ShowOption)choice;
                switch (showOption)
                {
                    case ShowOption.Station: baseStationView(); break;
                    case ShowOption.Drone: droneView(); break;
                    case ShowOption.Customer: customerView(); break;
                    case ShowOption.Parcel: parcelView(); break;
                    case ShowOption.Exit: break;
                    default: Console.WriteLine("error\n"); break;
                }
            }
        }
        /// <summary>
        /// the function prints a base station: it receives the ID number of the base station from the user, sends it to a function in DalObject
        /// which looks for this base station in the list and returns it, then, the function prints it. 
        /// if the function in DalObject didn't find it, the function prints a message to the user
        /// </summary>
        /// 
        public static void baseStationView()
        {
            Console.WriteLine("enter the base station ID");
            int idS;
            if (int.TryParse(Console.ReadLine(), out idS))
            {
                //DAL.IDAL.DO.BaseStation baseStation = DAL.DalObject.DalObject.ShowStation(idS);
                //if (baseStation.CodeStation == idS)
                //Console.WriteLine(baseStation + "\n");
                //else
                // Console.WriteLine("Sorry. The base station is not found\n");
                Console.WriteLine(dal.GetStation(idS));
            }
        }
        /// <summary>
        /// the function prints a drone: it receives the ID number of the drone from the user, sends it to a function in DalObject
        /// which looks for this drone in the list and returns it, then, the function prints it. 
        /// if the function in DalObject didn't find it, the function prints a message to the user
        /// </summary>
        public static void droneView()
        {
            Console.WriteLine("enter the drone ID");
            int idD;
            if (int.TryParse(Console.ReadLine(), out idD))
            {
                //DAL.IDAL.DO.Drone drone = DAL.DalObject.DalObject.ShowDrone(idD);
                // if (drone.CodeDrone == idD)
                //Console.WriteLine(drone + "\n");//לבדוק איך למנוע הדפסה במקרה שנזרקה חריגה, אולי לעשות כאן טריי
                // else
                // Console.WriteLine("Sorry. The drone is not found\n");
                Console.WriteLine(dal.GetDrone(idD));
            }
        }
        /// <summary>
        /// the function prints a customer: it receives the ID number of the customer from the user, sends it to a function in DalObject
        /// which looks for this customer in the list and returns it, then, the function prints it. 
        /// if the function in DalObject didn't find it, the function prints a message to the user
        /// </summary>
        public static void customerView()
        {
            Console.WriteLine("enter the customer ID");
            int idC;
            if (int.TryParse(Console.ReadLine(), out idC))
            {
                //DAL.IDAL.DO.Customer customer = DAL.DalObject.DalObject.ShowCustomer(idC);
                //if (customer.IdCustomer == idC)//איך לבדוק שהאובייקט לא ריק
                // Console.WriteLine(customer + "\n");
                //else
                // Console.WriteLine("Sorry. The customer is not found\n");
                Console.WriteLine(dal.GetCustomer(idC));
            }
        }
        /// <summary>
        /// the function prints a parcel: it receives the ID number of the parcel from the user, sends it to a function in DalObject
        /// which looks for this parcel in the list and returns it, then, the function prints it. 
        /// if the function in DalObject didn't find it, the function prints a message to the user
        /// </summary>
        public static void parcelView()
        {
            Console.WriteLine("enter the parcel ID");
            int idP;
            if (int.TryParse(Console.ReadLine(), out idP))
            {
                //DAL.IDAL.DO.Parcel parcel = DAL.DalObject.DalObject.ShowParcel(idP);
                //if (parcel.CodeParcel == idP)
                //  Console.WriteLine(parcel + "\n");
                // else
                //  Console.WriteLine("Sorry. The parcel is not found\n");
                Console.WriteLine(dal.GetParcel(idP));
            }
        }
        /// <summary>
        /// the function shows the options of view lists and cout the chose of the user
        /// </summary>
        public static void listViewOptions()
        {
            Console.WriteLine("Choose:\n1:a list of base stations view\n2:a list of drones view\n3:a list of customers view\n" +
                    "4:list of parcels view\n5:view of list of parcels not yet matched to a drone\n" +
                    "6:view of base stations with available charging stations\n");
            int choice;
            ShowListOption showListOption;
            if (int.TryParse(Console.ReadLine(), out choice))
            {
                showListOption = (ShowListOption)choice;
                switch (showListOption)
                {
                    case ShowListOption.Station: baseStationsListView(); break;
                    case ShowListOption.Drone: dronesListView(); break;
                    case ShowListOption.Customers: customersListView(); break;
                    case ShowListOption.Parcels: parcelsListView(); break;
                    case ShowListOption.UnAssignmentParcel: parcelsWithoutdrone(); break;
                    case ShowListOption.AvailableChargingsStation: baseStationsWithChargeSlots(); break;
                    default: Console.WriteLine("error\n"); break;
                }
            }
        }
        /// <summary>
        /// the function prints the list of base stations
        /// </summary>
        public static void baseStationsListView()
        {
            List<BaseStation> baseStations = dal.GetStationsByCondition().ToList();
            //for (int i = 1; i <= baseStations.Count; i++)
            //    Console.WriteLine("Base station No. " + i + ":\n" + baseStations[i]);
            foreach (BaseStation item in baseStations)
            {
                Console.WriteLine(item);
            }
        }
        /// <summary>
        /// the function prints the list of drones
        /// </summary>
        public static void dronesListView()
        {
            List<Drone> drones = dal.GetDronesByCondition().ToList();
            //for (int i = 1; i <= drones.Count; i++)
            //    Console.WriteLine("Drone No. " + i + ":\n" + drones[i]);
            foreach (Drone item in drones)
            {
                Console.WriteLine(item);
            }
        }
        /// <summary>
        /// the function prints the list of customers
        /// </summary>
        public static void customersListView()
        {
            List<Customer> customers = dal.GetCustomersByCondition().ToList();
            //for (int i = 1; i <= customers.Count; i++)
            //    Console.WriteLine("Customer No. " + i + ":\n" + customers[i]);
            foreach (Customer item in customers)
            {
                Console.WriteLine(item);
            }
        }
        /// <summary>
        /// the function prints the list of parcels
        /// </summary>
        public static void parcelsListView()
        {
            List<Parcel> parcels = dal.GetParcelsByCondition().ToList();
            //for (int i = 1; i <= parcels.Count; i++)
            //    Console.WriteLine("Parcel No. " + i + ":\n" + parcels[i]);
            foreach (Parcel item in parcels)
            {
                Console.WriteLine(item);
            }
        }
        /// <summary>
        /// the function prints the parcels that dowsn't connect the drone
        /// </summary>
        public static void parcelsWithoutdrone()
        {
            List<Parcel> parcels = dal.GetParcelsByCondition(parcel => parcel.DroneId == 0).ToList();
            //for (int i = 1; i <= parcels.Count; i++)
            //    Console.WriteLine("Parcel No. " + i + ":\n" + parcels[i]);
            foreach (Parcel item in parcels)
            {
                Console.WriteLine(item);
            }
        }
        /// <summary>
        /// the function prints the list of base stations that it has charge solts that are free
        /// </summary>
        public static void baseStationsWithChargeSlots()
        {
            List<BaseStation> baseStations = dal.GetStationsByCondition(station => station.ChargeSlots > 0).ToList();
            //for (int i = 1; i <= baseStations.Count; i++)
            //    Console.WriteLine("Parcel No. " + i + ":\n" + baseStations[i]);
            foreach (BaseStation item in baseStations)
            {
                Console.WriteLine(item);
            }
        }
        static void Main(string[] args)
        {
            int choice;
            Menu menu;
            do
            {
                Console.WriteLine("Choose one of the following:\n1-Add\n2-Update\n3-View\n4-View list\n0-Exit");
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    menu = (Menu)choice;
                    //choice = (Char)Console.Read();
                    switch (menu)
                    {
                        case Menu.Add: AddingOptions(); break;
                        case Menu.View: viewOptions(); break;
                        case Menu.ViewList: listViewOptions(); break;
                        case Menu.Exit: Console.WriteLine("bye\n"); break;
                        default: Console.WriteLine("error\n"); break;
                    }
                }
            }
            while (choice != 0);
        }

    }
}