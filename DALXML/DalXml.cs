using System;
using System.Collections.Generic;
using DalApi;
using DO;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dal
{
    sealed class DalXml : IDal
    {
        static readonly IDal instance = new DalXml();
        public static IDal Instance { get => instance; }
        //DalXml() { }
        XElement baseStationRoot;
        string baseStationPath = @"baseStationXml.xml";
        public DalXml()
        {
            if (!File.Exists(baseStationPath))
                CreateFiles();
            else
                LoadData();
        }
        private void CreateFiles()
        {
            baseStationRoot = new XElement("baseStations");
            baseStationRoot.Save(baseStationPath);
        }
        private void LoadData()
        {
            try
            {
                baseStationRoot = XElement.Load(baseStationPath);
            }
            catch
            {
                throw new Exception("File upload problem");
            }
        }
        public void SaveStationsListLinq(List<BaseStation> stationsList)
        {
            baseStationRoot = new XElement("baseStations",
                                        from bs in stationsList
                                        select new XElement("baseStation",
                                        new XElement("id", bs.CodeStation),
                                        new XElement("name", bs.NameStation),
                                        new XElement("numOfChargeSlots", bs.ChargeSlots),
                                        new XElement("location",
                                            new XElement("longitude", bs.Longitude),
                                            new XElement("latitude", bs.Latitude)
                                            )
                                        )
                                        );

            baseStationRoot.Save(baseStationPath);
        }
        // XElement ConvertBaseStation()
        #region base station function
        public void AddStation(BaseStation b)
        {
            BaseStation bs = GetStation(b.CodeStation);
            if(bs != null)
                throw new AlreadyExistException("The base station already exist in the system");
            XElement id = new XElement("id", b.CodeStation);
            XElement name= new XElement("name", b.NameStation);
            XElement numOfChargeSlots = new XElement("numOfChargeSlots", b.ChargeSlots);
            XElement longitude = new XElement("longitude", b.Longitude);
            XElement latitude = new XElement("latitude", b.Latitude);
            XElement location = new XElement("location", longitude, latitude);

            baseStationRoot.Add(new XElement("baseStation", id, name, numOfChargeSlots, location));
            baseStationRoot.Save(baseStationPath);
        }
        public void UpDateBaseStation(BaseStation b)
        {
            BaseStation baseStation = GetStation(b.CodeStation);
            if (baseStation == null)
                throw new DoesntExistException("This base station doesn't exist in the system");
            XElement baseStationElement = (from bs in baseStationRoot.Elements()
                                           where Convert.ToInt32(bs.Element("id").Value) == b.CodeStation
                                           select bs).FirstOrDefault();
            baseStationElement.Element("name").Value = Convert.ToString(b.NameStation);
            baseStationElement.Element("numOfChargeSlots").Value= Convert.ToString(b.ChargeSlots);
            baseStationElement.Element("location").Element("longitude").Value= Convert.ToString(b.Longitude);
            baseStationElement.Element("location").Element("latitude").Value= Convert.ToString(b.Latitude);

            baseStationRoot.Save(baseStationPath);
        }
        public BaseStation GetStation(int idS)
        {
            LoadData();
            BaseStation baseStation;
            try
            {
                baseStation = (from bs in baseStationRoot.Elements()
                             where Convert.ToInt32(bs.Element("id").Value) == idS
                               select new BaseStation()
                               {
                                   CodeStation = Convert.ToInt32(bs.Element("id").Value),
                                   NameStation = Convert.ToInt32(bs.Element("name").Value),
                                   ChargeSlots = Convert.ToInt32(bs.Element("numOfChargeSlots").Value),
                                   Longitude = Convert.ToDouble(bs.Element("location").Element("longitude").Value),
                                   Latitude = Convert.ToDouble(bs.Element("location").Element("latitude").Value),
                               }).FirstOrDefault();
            }
            catch
            {
                baseStation = default;
            }
            return baseStation;
        }
        public IEnumerable<BaseStation> GetStationsByCondition(Func<BaseStation, bool> conditionDelegate = null)
        {
            LoadData();
            IEnumerable<BaseStation> stations;
            try
            {
                stations = (from bs in baseStationRoot.Elements()
                            select new BaseStation()
                            {
                                CodeStation = Convert.ToInt32(bs.Element("id").Value),
                                NameStation = Convert.ToInt32(bs.Element("name").Value),
                                ChargeSlots = Convert.ToInt32(bs.Element("numOfChargeSlots").Value),
                                Longitude = Convert.ToDouble(bs.Element("location").Element("longitude").Value),
                                Latitude = Convert.ToDouble(bs.Element("location").Element("latitude").Value)
                            }
                          );
            }
            catch
            {
                stations = null;
            }
            return stations;
        }
        #endregion
        #region drone function
        public void AddDrone(Drone d)
        {
            throw new NotImplementedException();
        }
        #endregion
        public void AddCustomer(Customer c)
        {
            throw new NotImplementedException();
        }

        public void AddParcel(Parcel p)
        {
            throw new NotImplementedException();
        }

        public void AddDroneCharge(int idD, int idS, DateTime? dateTime)
        {
            throw new NotImplementedException();
        }

        public void RemoveDroneCharge(DroneCharge dc)
        {
            throw new NotImplementedException();
        }

        public void UpDateParcel(Parcel p)
        {
            throw new NotImplementedException();
        }

        public void UpDateDrone(Drone d)
        {
            throw new NotImplementedException();
        }

        public void UpDateCustomer(Customer c)
        {
            throw new NotImplementedException();
        }


        public Drone GetDrone(int idD)
        {
            throw new NotImplementedException();
        }

        public Customer GetCustomer(int idC)
        {
            throw new NotImplementedException();
        }

        public Parcel GetParcel(int idP)
        {
            throw new NotImplementedException();
        }

        public DroneCharge GetDroneCharge(int idD)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Parcel> GetParcelsByCondition(Func<Parcel, bool> conditionDelegate = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Drone> GetDronesByCondition(Func<Drone, bool> conditionDelegate = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Customer> GetCustomersByCondition(Func<Customer, bool> conditionDelegate = null)
        {
            throw new NotImplementedException();
        }

        public double[] AskElectrical()
        {
            throw new NotImplementedException();
        }
        public void RemoveParcel(int id)
        {
            throw new NotImplementedException();
        }
    }
}
