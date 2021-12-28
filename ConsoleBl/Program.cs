//using System;

//namespace ConsoleBl
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
using BlApi;
using BO;

namespace ConsoleBl
{
    class Program
    {
        static IBL bl = BlFactory.GetBl();
        enum MenuOption { Exit, Add, Update, View, ViewList }
        enum AddOption { Exit, Station, Drone, Customer, Parcel }
        enum UpdateOption { Exit, Drone, BaseStation, Customer, Charge, Release, Schedule, PickedUp, Delivery }
        enum ShowOption { Exit, Station, Drone, Customer, Parcel }
        enum ShowListOption { Exit, Stations, Drones, Customers, Parcels, UnAssignmentParcels, AvailableChargingsStations }
        public static Random rnd = new Random();
        static void Main(string[] args)
        {
            Menu();
        }
        private static void Menu()
        {
            int choice;
            MenuOption menu;
            do
            {
                Console.WriteLine("Choose one of the following:\n1-Add\n2-Update\n3-View\n4-View list\n0-Exit");
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    menu = (MenuOption)choice;
                    //choice = (Char)Console.Read();
                    switch (menu)
                    {
                        case MenuOption.Add: Program.AddingOptions(); break;
                        case MenuOption.Update: Program.UpdateOptions(); break;
                        case MenuOption.View: Program.ViewOptions(); break;
                        case MenuOption.ViewList: Program.ListViewOptions(); break;
                        case MenuOption.Exit: Console.WriteLine("bye\n"); break;
                        default: Console.WriteLine("error\n"); break;
                    }
                }
            }
            while (choice != 0);
        }
        public static void AddingOptions()
        {
            AddOption addOp;
            int choice;
            Console.WriteLine("Choose:\n1-add base station\n2-add drone\n3-add customer\n4-add parcel\n0-exit");
            if (int.TryParse(Console.ReadLine(), out choice))
            {
                addOp = (AddOption)choice;
                {
                    switch (addOp)
                    {
                        case AddOption.Station: NewBaseStation(); break;
                        case AddOption.Drone: NewDrone(); break;
                        case AddOption.Customer: NewCustomer(); break;
                        case AddOption.Parcel: NewParcel(); break;
                        case AddOption.Exit: break;
                        default: Console.WriteLine("error\n"); break;
                    }
                }
            }
        }
        public static void NewBaseStation()
        {
            try
            {
                Console.WriteLine("enter ID number, name(with digits only), longitude ,latitude ,number of charging positions ");
                int id; int name; int chargeSlots;
                if (int.TryParse(Console.ReadLine(), out id))
                    if (int.TryParse(Console.ReadLine(), out name))
                    {
                        double tempLon = double.Parse(Console.ReadLine());
                        double tempLat = double.Parse(Console.ReadLine());
                        if (int.TryParse(Console.ReadLine(), out chargeSlots))
                        {
                            double longitude = (double)tempLon;
                            double lattitude = (double)tempLat;
                            BO.BaseStation baseStation = new BO.BaseStation()
                            {
                                Id = id,
                                Name = name,
                                ChargeSlots = chargeSlots,
                                Location = new Location(tempLon, tempLat),
                                ListDroneCharge = new List<DroneInCharge>(),
                            };
                            bl.AddBaseStation(baseStation);
                        }
                    }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n");
            }

        }
        public static void NewDrone()
        {
            try
            {
                Console.WriteLine("enter ID number, model, max weight(easy=0, medium=1, heavy=2) of the drone, id of station for charge\n");
                int id; int enumWeight; int idStation;
                if (int.TryParse(Console.ReadLine(), out id))
                {
                    string model = Console.ReadLine();
                    if (int.TryParse(Console.ReadLine(), out enumWeight))
                    {
                        WeightCategories weight = (WeightCategories)enumWeight;
                        if (int.TryParse(Console.ReadLine(), out idStation))
                        {
                            BO.Drone drone = new BO.Drone()
                            {
                                Id = id,
                                ModelDrone = model,
                                MaxWeight = weight,
                            };
                            bl.AddDrone(drone, idStation);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n");
            }

        }
        public static void NewCustomer()
        {
            try
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
                    BO.Customer customer = new BO.Customer()
                    {
                        Id = id,
                        Name = name,
                        Phone = phone,
                        Location = new Location(longitude, lattitude),
                    };
                    bl.AddCustomer(customer);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n");
            }
        }
        public static void NewParcel()
        {
            try
            {
                Console.WriteLine("enter parcel ID number, sender ID number, target customer ID number,weight(easy=0, medium=1, heavy=2) and priority (normal=0, express=1, emergency=2) of the parcel");
                int idParcel; int idSender; int idTarget;
                if (int.TryParse(Console.ReadLine(), out idParcel))
                    if (int.TryParse(Console.ReadLine(), out idSender))
                        if (int.TryParse(Console.ReadLine(), out idTarget))
                        {
                            WeightCategories weight = (WeightCategories)Console.Read();
                            Priorities priority = (Priorities)Console.Read();
                            BO.Parcel parcel = new BO.Parcel()
                            {
                                CodeParcel = idParcel,
                                SenderCustomer = new CustomerInParcel() { Id = idSender },
                                TargetCustomer = new CustomerInParcel() { Id = idTarget },
                                Weight = weight,
                                Priority = priority
                            };
                            bl.AddParcel(parcel);
                        }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n");
            }
        }
        /// <summary>
        /// view of the update possibilities for the user and going to the matching update function according to his choice
        /// </summary>
        public static void UpdateOptions()
        {
            UpdateOption updateOp;
            int choice;
            Console.WriteLine("Choose:\n1-update drone\n2-update base station\n3-update customer\n4-update sending drone to charge\n" +
                "5-update releasing drone from charge\n6-update scheduling parcel to drone\n7-update picking up parcel by drone\n" +
                "8-update delivering parcel by drone\n0-exit");
            if (int.TryParse(Console.ReadLine(), out choice))
            {
                updateOp = (UpdateOption)choice;
                {
                    switch (updateOp)
                    {
                        case UpdateOption.Drone: UpdateDrone(); break;
                        case UpdateOption.BaseStation: UpdateStation(); break;
                        case UpdateOption.Customer: UpdateCustomer(); break;
                        case UpdateOption.Charge: UpdateSendingDroneToCharge(); break;
                        case UpdateOption.Release: UpdateReleasingDroneFromCharge(); break;
                        case UpdateOption.Schedule: UpdateSchedulingParcelToDrone(); break;
                        case UpdateOption.PickedUp: UpdatePickingUpParcelByDrone(); break;
                        case UpdateOption.Delivery: UpdateDeliveringParcelByDrone(); break;
                        case UpdateOption.Exit: break;
                        default: Console.WriteLine("error\n"); break;
                    }
                }
            }
        }
        public static void UpdateDrone()
        {
            try
            {
                Console.WriteLine("enter drone ID number and a new model");
                int idD;
                if (int.TryParse(Console.ReadLine(), out idD))
                {
                    string newModel = Console.ReadLine();
                    bl.UpdateDrone(idD, newModel);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n");
            }
        }
        public static void UpdateStation()
        {
            try
            {
                Console.WriteLine("Enter base station ID number.\n Enter one or more of the following details you want to update:\n " +
                    " new name for the base station(with digits only) and number of charge positios in the station.\n" +
                    "In case you do not want to update one of the details, press 0 for it");
                int idS; int newName; int numOfChargePositions;
                if (int.TryParse(Console.ReadLine(), out idS))
                    if (int.TryParse(Console.ReadLine(), out newName))
                        if (int.TryParse(Console.ReadLine(), out numOfChargePositions))
                            bl.UpdateBaseStation(idS, newName, numOfChargePositions);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n");
            }
        }
        public static void UpdateCustomer()
        {
            try
            {
                Console.WriteLine("Enter customer ID number.\n Enter one or more of the following details you want to update:\n " +
                    " new name for the customer and new phone number.\n" +
                    "In case you do not want to update one of the details, press 0 for it");
                int idC;
                if (int.TryParse(Console.ReadLine(), out idC))
                {
                    string newName = Console.ReadLine();
                    string newPhone = Console.ReadLine();
                    bl.UpdateCustomer(idC, newName, newPhone);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n");
            }
        }
        public static void UpdateSendingDroneToCharge()
        {
            try
            {
                Console.WriteLine("Enter drone ID number");
                int idD;
                if (int.TryParse(Console.ReadLine(), out idD))
                    bl.UpdateSendingDroneToCharge(idD);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n");
            }
        }
        public static void UpdateReleasingDroneFromCharge()
        {
            try
            {
                Console.WriteLine("Enter drone ID number and the amount of time the drone has been charging");
                int idD;
                if (int.TryParse(Console.ReadLine(), out idD))
                {
                    double chargingTime = double.Parse(Console.ReadLine());
                    bl.UpdateReleasingDroneFromCharge(idD/*, chargingTime*/);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n");
            }
        }
        public static void UpdateSchedulingParcelToDrone()
        {
            try
            {
                Console.WriteLine("Enter drone ID number");
                int idD;
                if (int.TryParse(Console.ReadLine(), out idD))
                    bl.UpdateParcelToDrone(idD);
            }
            catch (UpdateProblemException ex)
            {
                Console.WriteLine(ex + "\n");
            }
        }
        public static void UpdatePickingUpParcelByDrone()
        {
            try
            {
                Console.WriteLine("Enter drone ID number");
                int idD;
                if (int.TryParse(Console.ReadLine(), out idD))
                    bl.UpdateParcelPickedUpByDrone(idD);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n");
            }
        }
        public static void UpdateDeliveringParcelByDrone()
        {
            try
            {
                Console.WriteLine("Enter drone ID number");
                int idD;
                if (int.TryParse(Console.ReadLine(), out idD))
                    bl.UpdateDeliveredParcelByDrone(idD);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n");
            }
        }
        public static void ViewOptions()
        {
            ShowOption showOp;
            int choice;
            Console.WriteLine("Choose:\n1-show a base station\n2-show a drone\n3-show a customer\n4-show a parcel\n0-exit");
            if (int.TryParse(Console.ReadLine(), out choice))
            {
                showOp = (ShowOption)choice;
                {
                    switch (showOp)
                    {
                        case ShowOption.Station: ShowBaseStation(); break;
                        case ShowOption.Drone: ShowDrone(); break;
                        case ShowOption.Customer: ShowCustomer(); break;
                        case ShowOption.Parcel: ShowParcel(); break;
                        case ShowOption.Exit: break;
                        default: Console.WriteLine("error\n"); break;
                    }
                }
            }
        }
        public static void ShowBaseStation()
        {
            try
            {
                Console.WriteLine("Enter base station ID number");
                int idS;
                if (int.TryParse(Console.ReadLine(), out idS))
                    Console.WriteLine(bl.GetBaseStation(idS));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n");
            }
        }
        public static void ShowDrone()
        {
            try
            {
                Console.WriteLine("Enter drone ID number");
                int idD;
                if (int.TryParse(Console.ReadLine(), out idD))
                {
                    Drone d = bl.GetDrone(idD);
                    Console.WriteLine(d);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex + "\n");

            }
        }
        public static void ShowCustomer()
        {
            try
            {
                Console.WriteLine("Enter customer ID number");
                int idC;
                if (int.TryParse(Console.ReadLine(), out idC))
                    Console.WriteLine(bl.GetCustomer(idC));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n");
            }
        }
        public static void ShowParcel()
        {
            try
            {
                Console.WriteLine("Enter parcel ID number");
                int idP;
                if (int.TryParse(Console.ReadLine(), out idP))
                    Console.WriteLine(bl.GetParcel(idP));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n");
            }
        }
        public static void ListViewOptions()
        {
            ShowListOption showListOp;
            int choice;
            Console.WriteLine("Choose:\n1-show all the base stations\n2-show all the drones\n3-show all the customers\n" +
                "4-show all the parcels\n5-show all the parcels that were not scheduled to a drone\n6-show all the base stations with free charge positions\n0-exit");
            if (int.TryParse(Console.ReadLine(), out choice))
            {
                showListOp = (ShowListOption)choice;
                {
                    switch (showListOp)
                    {
                        case ShowListOption.Stations: ShowAllBaseStations(); break;
                        case ShowListOption.Drones: ShowAllDrones(); break;
                        case ShowListOption.Customers: ShowAllCustomers(); break;
                        case ShowListOption.Parcels: ShowAllParcels(); break;
                        case ShowListOption.UnAssignmentParcels: ShowAllUnAssignmentParcels(); break;
                        case ShowListOption.AvailableChargingsStations: ShowAllAvailableChargingsStations(); break;
                        case ShowListOption.Exit: break;
                        default: Console.WriteLine("error\n"); break;
                    }
                }
            }
        }
        public static void ShowAllBaseStations()
        {
            try
            {
                List<BaseStationForList> baseStationsList = bl.GetBaseStations().ToList();
                foreach (BaseStationForList item in baseStationsList)
                {
                    Console.WriteLine(item);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n");
            }
        }
        public static void ShowAllDrones()
        {
            try
            {
                List<DroneForList> dronesList = bl.GetDrones().ToList();
                foreach (DroneForList item in dronesList)
                {
                    Console.WriteLine(item);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n");
            }
        }
        public static void ShowAllCustomers()
        {
            try
            {
                List<CustomerForList> customersList = bl.GetCustomers().ToList();
                foreach (CustomerForList item in customersList)
                {
                    Console.WriteLine(item);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n");
            }
        }
        public static void ShowAllParcels()
        {
            try
            {
                List<ParcelForList> parcelsList = bl.GetParcels().ToList();
                foreach (ParcelForList item in parcelsList)
                {
                    Console.WriteLine(item);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n");
            }
        }
        public static void ShowAllUnAssignmentParcels()
        {
            try
            {
                List<ParcelForList> parcelsWithoutDroneList = bl.GetAllParcelsWithoutDrone().ToList();
                foreach (ParcelForList item in parcelsWithoutDroneList)
                {
                    Console.WriteLine(item);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n");
            }
        }
        public static void ShowAllAvailableChargingsStations()
        {
            try
            {
                List<BaseStationForList> baseStationsWithChargePositionsList = bl.GetAllBaseStationsWithChargePositions().ToList();
                foreach (BaseStationForList item in baseStationsWithChargePositionsList)
                {
                    Console.WriteLine(item);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n");
            }
        }
    }
}



