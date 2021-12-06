using System;
using System.Collections.Generic;
using IBL.BO;

namespace ConsoleUI_BL
{
   public  class Program
    {
        public  Random rnd = new Random();
        public  IBL.IBl bl = new IBL.BLObject();
        enum Menu { Exit, Add, Update, View, ViewList }
        enum AddOption { Exit, Station, Drone, Customer, Parcel }
        enum UpdateOption { Exit, Drone, BaseStation, Customer, Charge , Release, Schedule, PickedUp, Delivery }
        enum ShowOption { Exit, Station, Drone, Customer, Parcel }
        enum ShowListOption { Exit, Stations, Drones, Customers, Parcels, UnAssignmentParcels, AvailableChargingsStations }
        /// <summary>
        /// the function offers the user 4 adding options for adding base station/drone/customer/parcel.
        /// </summary>
        public  void AddingOptions()
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
        public void NewBaseStation()
        {
            try
            {
                Console.WriteLine("enter ID number, name, longitude ,latitude ,number of charging positions ");
                int id; int name; int chargeSlots;
                if (int.TryParse(Console.ReadLine(), out id))
                    if (int.TryParse(Console.ReadLine(), out name))
                        if (int.TryParse(Console.ReadLine(), out chargeSlots))
                        {
                            int tempLon = int.Parse(Console.ReadLine());
                            int tempLat = int.Parse(Console.ReadLine());
                            double longitude = (double)tempLon;
                            double lattitude = (double)tempLat;
                            IBL.BO.BaseStation baseStation = new IBL.BO.BaseStation()
                            {
                                Id = id,
                                Name = name,
                                ChargeSlots = chargeSlots,
                                Location = new Location(tempLon, tempLat),
                                ListDroneCharge = new List<DroneCharge>(),
                            };
                            bl.AddBaseStation(baseStation);
                        }
            }
            catch(AddingProblemException ex)
            {
                Console.WriteLine(ex);
            }

        }
        public void NewDrone()
        {
            try
            {
                Console.WriteLine("enter ID number, model, max weight(easy, medium, heavy) of the drone, id of station for charge\n");
                int id; int idStation;
                if (int.TryParse(Console.ReadLine(), out id))
                {
                    string model = Console.ReadLine();
                    WeightCategories weight = (WeightCategories)Console.Read();
                    if (int.TryParse(Console.ReadLine(), out idStation))
                    {
                        IBL.BO.Drone drone = new IBL.BO.Drone()
                        {
                            Id = id,
                            ModelDrone = model,
                            MaxWeight = weight,
                        };
                        bl.AddDrone(drone, idStation);
                    }

                }
            }
            catch(AddingProblemException ex)
            {
                Console.WriteLine(ex);
            }

        }
        public void NewCustomer()
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
                    IBL.BO.Customer customer = new IBL.BO.Customer()
                    {
                        Id = id,
                        Name = name,
                        Phone = phone,
                        Location = new Location(longitude, lattitude),
                    };
                    bl.AddCustomer(customer);
                }
            }
            catch(AddingProblemException ex)
            {
                Console.WriteLine(ex);
            }
        }
        public void NewParcel()
        {
            try 
            { 
            Console.WriteLine("enter parcel ID number, sender ID number, target customer ID number,weight(easy, medium, heavy) and priority (normal, express, emergency) of the parcel");
            int idP; int idS; int idT;
                if (int.TryParse(Console.ReadLine(), out idP))
                    if (int.TryParse(Console.ReadLine(), out idS))
                        if (int.TryParse(Console.ReadLine(), out idT))
                        {
                            WeightCategories weight = (WeightCategories)Console.Read();
                            Priorities priority = (Priorities)Console.Read();
                            DateTime creatingTime = DateTime.Now;
                            IBL.BO.Parcel parcel = new IBL.BO.Parcel()
                            {
                                CodeParcel = idP,
                                SenderCustomerId = idS,
                                TargetCustomerId = idT,
                                Weight = weight,
                                Priority = priority
                            };
                            bl.AddParcel(parcel);
                        }
            }
            catch(AddingProblemException ex)
            {
                Console.WriteLine(ex);
            }
        }
       public void Main(string[] args)
        {
            Program p = new Program();
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
                        case Menu.Add: p.AddingOptions(); break;
                        case Menu.Update: p.; break;
                        case Menu.View: p.ViewOptions(); break;
                        case Menu.ViewList: ListViewOptions(); break;
                        case Menu.Exit: Console.WriteLine("bye\n"); break;
                        default: Console.WriteLine("error\n"); break;
                    }
                }
            }
            while (choice != 0);

        }
    }
}
