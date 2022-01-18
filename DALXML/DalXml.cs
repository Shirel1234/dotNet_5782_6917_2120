using System;
using System.Collections.Generic;
using System.Linq;
using DalApi;
using DO;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dal
{
    sealed class DalXml : IDal
    {
        #region fields and singelton
        static readonly IDal instance = new DalXml();
        public static IDal Instance { get => instance; }
        XElement baseStationRoot;
        string baseStationPath = @"StationsXml.xml";
        string dronesPath = @"DronesXml.xml";//XMLSerializer
        string customersPath = @"CustomersXml.xml";//XMLSerializer
        string parcelsPath = @"ParcelsXml.xml";//XMLSerializer
        string dronesChargePath = @"DronesChargeXml.xml";//XMLSerializer
        string runNumberPath = @"RunNumbers.xml";//XMLSerializer
        #endregion
        private DalXml()
        {
           //In the first time
           //DataSourceXml.Initialize();
           baseStationRoot = XMLTools.LoadListFromXMLElement(baseStationPath);
        }
        #region Adding
        /// <summary>
        /// add a base station
        /// </summary>
        /// <param name="b">a base station</param>
        public void AddStation(BaseStation b)
        {
            BaseStation bs = GetStation(b.CodeStation);
            if(bs.CodeStation!= 0)
                throw new AlreadyExistException("The base station already exist in the system");
            XElement id = new XElement("id", b.CodeStation);
            XElement name= new XElement("name", b.NameStation);
            XElement numOfChargeSlots = new XElement("numOfChargeSlots", b.ChargeSlots);
            XElement longitude = new XElement("longitude", b.Longitude);
            XElement latitude = new XElement("latitude", b.Latitude);
            XElement location = new XElement("location", longitude, latitude);

            baseStationRoot.Add(new XElement("Station", id, name, numOfChargeSlots, location));
            XMLTools.SaveListToXMLElement(baseStationRoot, baseStationPath);
        }
        /// <summary>
        /// add a drone
        /// </summary>
        /// <param name="drone">a drone</param>
        public void AddDrone(Drone drone)
        {
            List<Drone> droneList = XMLTools.LoadListFromXMLSerializer<Drone>(dronesPath);
            Drone dron1 = droneList.FirstOrDefault(d => d.CodeDrone == drone.CodeDrone);

            if (dron1.CodeDrone != 0)
            {
                throw new AlreadyExistException("The drone already exist in the system");
            }

            droneList.Add(drone);

            XMLTools.SaveListToXMLSerializer<Drone>(droneList, dronesPath);
        }
        /// <summary>
        /// add a customer
        /// </summary>
        /// <param name="customer"> a customer</param>
        public void AddCustomer(Customer customer)
        {
            List<Customer> customerList = XMLTools.LoadListFromXMLSerializer<Customer>(customersPath);

            Customer cust1 = customerList.FirstOrDefault(c=> c.IdCustomer == customer.IdCustomer);

            if (cust1.IdCustomer != 0)
            {
                throw new AlreadyExistException("The customer already exist in the system");
            }

            customerList.Add(customer);

            XMLTools.SaveListToXMLSerializer<Customer>(customerList, customersPath);
        }
        /// <summary>
        /// add a parcel
        /// </summary>
        /// <param name="parcel">a parcel</param>
        public void AddParcel(Parcel parcel)
        {
            List<Parcel> parcelList = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
            List<int> runNumbers = XMLTools.LoadListFromXMLSerializer<int>(runNumberPath);
            parcel.CodeParcel = runNumbers[0]++;
            parcelList.Add(parcel);

            XMLTools.SaveListToXMLSerializer<Parcel>(parcelList, parcelsPath);
            XMLTools.SaveListToXMLSerializer<int>(runNumbers, runNumberPath);
        }
        /// <summary>
        /// add a droneCharge
        /// </summary>
        /// <param name="idD">id of the drone</param>
        /// <param name="idS">id of the station that the drone is in charge there</param>
        /// <param name="dateTime">the time of starting the charging</param>
        public void AddDroneCharge(int idD, int idS, DateTime? dateTime)
        {
            List<DroneCharge> droneChargeList = XMLTools.LoadListFromXMLSerializer<DroneCharge>(dronesChargePath);
            droneChargeList.Add(new DroneCharge
            {
                StationID=idS,
                DroneID=idD,
                BeginingCharge=dateTime
            });
            XMLTools.SaveListToXMLSerializer<DroneCharge>(droneChargeList, dronesChargePath);
        }
        #endregion

        #region Updateing
        /// <summary>
        /// update a base station
        /// </summary>
        /// <param name="b">an updated base station</param>
        public void UpDateBaseStation(BaseStation b)
        {
            BaseStation baseStation = GetStation(b.CodeStation);
            if (baseStation.CodeStation == 0)
                throw new DoesntExistException("This base station doesn't exist in the system");
            XElement baseStationElement = (from bs in baseStationRoot.Elements()
                                           where Convert.ToInt32(bs.Element("id").Value) == b.CodeStation
                                           select bs).FirstOrDefault();
            baseStationElement.Element("name").Value = Convert.ToString(b.NameStation);
            baseStationElement.Element("numOfChargeSlots").Value = Convert.ToString(b.ChargeSlots);
            baseStationElement.Element("location").Element("longitude").Value = Convert.ToString(b.Longitude);
            baseStationElement.Element("location").Element("latitude").Value = Convert.ToString(b.Latitude);

            baseStationRoot.Save(baseStationPath);
        }
        /// <summary>
        /// update a parcel
        /// </summary>
        /// <param name="parcel">an updated parcel</param>
        public void UpDateParcel(Parcel parcel)
        {
            List<Parcel> parcelList = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);

            int index = parcelList.FindIndex(p => p.CodeParcel == parcel.CodeParcel);

            if (index == -1)
                throw new DoesntExistException("This parcel doesn't exist in the system");
            parcelList[index] = parcel;
            XMLTools.SaveListToXMLSerializer<Parcel>(parcelList, parcelsPath);
        }
        /// <summary>
        /// update a drone
        /// </summary>
        /// <param name="drone">an updated drone</param>
        public void UpDateDrone(Drone drone)
        {
            List<Drone> droneList = XMLTools.LoadListFromXMLSerializer<Drone>(dronesPath);

            int index = droneList.FindIndex(d => d.CodeDrone == drone.CodeDrone);

            if (index == -1)
                throw new DoesntExistException("This drone doesn't exist in the system");
            droneList[index] = drone;
            XMLTools.SaveListToXMLSerializer<Drone>(droneList, dronesPath);
        }
        /// <summary>
        /// update a customer
        /// </summary>
        /// <param name="customer">an updated customer</param>
        public void UpDateCustomer(Customer customer)
        {
            List<Customer> customerList = XMLTools.LoadListFromXMLSerializer<Customer>(customersPath);

            int index = customerList.FindIndex(c => c.IdCustomer == customer.IdCustomer);

            if (index == -1)
                throw new DoesntExistException("This customer doesn't exist in the system");

            customerList[index] = customer;

            XMLTools.SaveListToXMLSerializer<Customer>(customerList, customersPath);

        }

        #endregion

        #region Removeing
        /// <summary>
        /// remove a drone in charging
        /// </summary>
        /// <param name="dc">a droneCharge</param>
        public void RemoveDroneCharge(DroneCharge dc)
        {
            List<DroneCharge> droneChargeList = XMLTools.LoadListFromXMLSerializer<DroneCharge>(dronesChargePath);

            //DroneCharge droneCharge = GetDroneCharge(dc.DroneID);

             droneChargeList.Remove(dc);

            XMLTools.SaveListToXMLSerializer<DroneCharge>(droneChargeList, dronesChargePath);
        }
        /// <summary>
        /// remove a parcel
        /// </summary>
        /// <param name="id">id of the parcel to remove</param>
        public void RemoveParcel(int id)
        {
            List<Parcel> parcelList = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);

            Parcel parc1 = GetParcel(id);

            if (parc1.CodeParcel == 0)
            {
                throw new DoesntExistException("This parcel does not exist");
            }
            parcelList.Remove(parc1);

            XMLTools.SaveListToXMLSerializer<Parcel>(parcelList, parcelsPath);
        }
        #endregion

        #region Getting
        /// <summary>
        /// get a single base station according to its id number
        /// </summary>
        /// <param name="idS">id number of base station</param>
        /// <returns>a single base station</returns>
        public BaseStation GetStation(int idS)
        {
            XMLTools.LoadListFromXMLElement(baseStationPath);
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

        /// <summary>
        /// get a single drone according to its id number
        /// </summary>
        /// <param name="id">id number of drone</param>
        /// <returns>a single drone</returns>
        public Drone GetDrone(int id)
        {
            List<Drone> droneList = XMLTools.LoadListFromXMLSerializer<Drone>(dronesPath);
            int index = droneList.FindIndex(d => d.CodeDrone == id);
            if (index == -1)
                throw new DoesntExistException("This drone does not exist");
            return droneList.FirstOrDefault(d => d.CodeDrone == id);
        }
        /// <summary>
        /// get a single customer according to its id number
        /// </summary>
        /// <param name="id">id number of customer</param>
        /// <returns>a single customer</returns>
        public Customer GetCustomer(int id)
        {
            List<Customer> customerList = XMLTools.LoadListFromXMLSerializer<Customer>(customersPath);
            int index = customerList.FindIndex(c => c.IdCustomer == id);
            if (index == -1)
                throw new DoesntExistException("This customer does not exist");
            return customerList.FirstOrDefault(c => c.IdCustomer == id);
        }
        /// <summary>
        /// get a single parcel according to its id number
        /// </summary>
        /// <param name="id">id number of parcel</param>
        /// <returns>a single parcel</returns>
        public Parcel GetParcel(int id)
        {
            List<Parcel> parcelList = GetParcelsByCondition().ToList();
            int index = parcelList.FindIndex(p => p.CodeParcel == id);
            if (index == -1)
                throw new DoesntExistException("This parcel does not exist");
            return parcelList.FirstOrDefault(p => p.CodeParcel == id);
        }
        /// <summary>
        /// get a single DroneCharge according to its id number
        /// </summary>
        /// <param name="id">id number of DroneCharge</param>
        /// <returns>a single DroneCharge</returns>
        public DroneCharge GetDroneCharge(int id)
        {
            List<DroneCharge> droneChargelList = XMLTools.LoadListFromXMLSerializer<DroneCharge>(dronesChargePath);
            int index = droneChargelList.FindIndex(dc => dc.DroneID == id);
            if (index == -1)
                throw new DoesntExistException("This droneCharge does not exist");
            return droneChargelList.FirstOrDefault(dc => dc.DroneID == id);
        }
        #endregion

        #region GettingAllByCondition
        /// <summary>
        /// get all the base stations that answer to this condition, if exists
        /// </summary>
        /// <param name="conditionDelegate">the condition</param>
        /// <returns>all the base stations</returns>
        public IEnumerable<BaseStation> GetStationsByCondition(Func<BaseStation, bool> conditionDelegate = null)
        {
            baseStationRoot=XMLTools.LoadListFromXMLElement(baseStationPath);
            IEnumerable<BaseStation> stations;
            stations = (from bs in baseStationRoot.Elements()
                            select new BaseStation()
                            {
                                CodeStation = Convert.ToInt32(bs.Element("id").Value),
                                NameStation = Convert.ToInt32(bs.Element("name").Value),
                                ChargeSlots = Convert.ToInt32(bs.Element("numOfChargeSlots").Value),
                                Longitude = Convert.ToDouble(bs.Element("location").Element("longitude").Value),
                                Latitude = Convert.ToDouble(bs.Element("location").Element("latitude").Value)
                            });
            return stations;
        }
        /// <summary>
        /// get all the parcels that answer to this condition, if exists
        /// </summary>
        /// <param name="conditionDelegate">the condition</param>
        /// <returns>all the parcels</returns>
        public IEnumerable<Parcel> GetParcelsByCondition(Func<Parcel, bool> conditionDelegate = null)
         {
                List<Parcel> parcelList = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
                var v = from item in parcelList
                        select item; 
                if (conditionDelegate == null)
                    return v.AsEnumerable().OrderByDescending(p => p.CodeParcel);
                return v.Where(conditionDelegate).OrderByDescending(p => p.CodeParcel);
        }
        /// <summary>
        /// get all the drones that answer to this condition, if exists
        /// </summary>
        /// <param name="conditionDelegate">the condition</param>
        /// <returns>all the drones</returns>
        public IEnumerable<Drone> GetDronesByCondition(Func<Drone, bool> conditionDelegate = null)
        {
            List<Drone> droneList = XMLTools.LoadListFromXMLSerializer<Drone>(dronesPath);
            var v = from item in droneList
                    select item; //item.Clone();
            if (conditionDelegate == null)
                return v.AsEnumerable().OrderByDescending(d => d.CodeDrone);
            return v.Where(conditionDelegate).OrderByDescending(d => d.CodeDrone);
        }
        /// <summary>
        /// get all the customers that answer to this condition, if exists
        /// </summary>
        /// <param name="conditionDelegate">the condition</param>
        /// <returns>all the customers</returns>
        public IEnumerable<Customer> GetCustomersByCondition(Func<Customer, bool> conditionDelegate = null)
        {
            List<Customer> customerList = XMLTools.LoadListFromXMLSerializer<Customer>(customersPath);
            var v = from item in customerList
                    select item; //item.Clone();
            if (conditionDelegate == null)
                return v.AsEnumerable().OrderByDescending(c => c.IdCustomer);
            return v.Where(conditionDelegate).OrderByDescending(c => c.IdCustomer);
        }
        /// <summary>
        /// get all the drones in charge that answer to this condition, if exists
        /// </summary>
        /// <param name="conditionDelegate">the condition</param>
        /// <returns>all the drones in charge</returns>
        public IEnumerable<DroneCharge> GetDronesChargeByCondition(Func<DroneCharge, bool> conditionDelegate = null)
        {
            List<DroneCharge> droneChargeList = XMLTools.LoadListFromXMLSerializer<DroneCharge>(dronesChargePath);
            var v = from item in droneChargeList
                    select item; //item.Clone();
            if (conditionDelegate == null)
                return v.AsEnumerable().OrderByDescending(dc => dc.DroneID);
            return v.Where(conditionDelegate).OrderByDescending(dc => dc.DroneID);
        }
        #endregion
        public double[] AskElectrical()
        {
            double[] arrElectrical = new double[5];
            arrElectrical[0] = DataSourceXml.Config.Free;
            arrElectrical[1] = DataSourceXml.Config.Easy;
            arrElectrical[2] = DataSourceXml.Config.Medium;
            arrElectrical[3] = DataSourceXml.Config.Heavy;
            arrElectrical[4] = DataSourceXml.Config.ChargingRate;
            return arrElectrical;
        }
    }
}