﻿using System;
using System.Collections.Generic;
using System.Linq;
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
        #region singelton
        static readonly IDal instance = new DalXml();
        public static IDal Instance { get => instance; }
        XElement baseStationRoot;
        string baseStationPath = @"StationsXml.xml";
        string dronesPath = @"DronesXml.xml";//XMLSerializer
        string customersPath = @"CustomersXml.xml";//XMLSerializer
        string parcelsPath = @"ParcelsXml.xml";//XMLSerializer
        string dronesChargePath = @"DronesChargeXml.xml";//XMLSerializer
        #endregion
        private DalXml()
        {
            //In the first time
            DataSourceXml.Initialize();

        }
        #region Adding
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

        public void AddParcel(Parcel parcel)
        {
            List<Parcel> parcelList = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);

            Parcel parc1 = parcelList.FirstOrDefault(p => p.CodeParcel == parcel.CodeParcel);

            if (parc1.CodeParcel != 0)
            {
                throw new AlreadyExistException("The parcel already exist in the system");
            }

            parcelList.Add(parcel);

            XMLTools.SaveListToXMLSerializer<Parcel>(parcelList, parcelsPath);
        }
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
        public void UpDateParcel(Parcel parcel)
        {
            List<Parcel> parcelList = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);

            int index = parcelList.FindIndex(p => p.CodeParcel == parcel.CodeParcel);

            if (index == -1)
                throw new DoesntExistException("This parcel doesn't exist in the system");
            parcelList[index] = parcel;
            XMLTools.SaveListToXMLSerializer<Parcel>(parcelList, parcelsPath);
        }

        public void UpDateDrone(Drone drone)
        {
            List<Drone> droneList = XMLTools.LoadListFromXMLSerializer<Drone>(dronesPath);

            int index = droneList.FindIndex(d => d.CodeDrone == drone.CodeDrone);

            if (index == -1)
                throw new DoesntExistException("This drone doesn't exist in the system");
            droneList[index] = drone;
            XMLTools.SaveListToXMLSerializer<Drone>(droneList, dronesPath);
        }

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
        public void RemoveDroneCharge(DroneCharge dc)
        {
            List<DroneCharge> droneChargeList = XMLTools.LoadListFromXMLSerializer<DroneCharge>(dronesChargePath);

            DroneCharge droneCharge = GetDroneCharge(dc.DroneID);

            if (droneCharge.DroneID == 0)
            {
                throw new DoesntExistException("This drone charge does not exist");
            }
             droneChargeList.Remove(droneCharge);

            XMLTools.SaveListToXMLSerializer<DroneCharge>(droneChargeList, dronesChargePath);
        }
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


        public Drone GetDrone(int id)
        {
            List<Drone> droneList = XMLTools.LoadListFromXMLSerializer<Drone>(dronesPath);
            int index = droneList.FindIndex(d => d.CodeDrone == id);
            if (index == -1)
                throw new DoesntExistException("This drone does not exist");
            return droneList.FirstOrDefault(d => d.CodeDrone == id);
        }
        public Customer GetCustomer(int id)
        {
            List<Customer> customerList = XMLTools.LoadListFromXMLSerializer<Customer>(customersPath);
            int index = customerList.FindIndex(c => c.IdCustomer == id);
            if (index == -1)
                throw new DoesntExistException("This customer does not exist");
            return customerList.FirstOrDefault(c => c.IdCustomer == id);
        }

        public Parcel GetParcel(int id)
        {
            List<Parcel> parcelList = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
            int index = parcelList.FindIndex(p => p.CodeParcel == id);
            if (index == -1)
                throw new DoesntExistException("This parcel does not exist");
            return parcelList.FirstOrDefault(p => p.CodeParcel == id);
        }

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


        public IEnumerable<Parcel> GetParcelsByCondition(Func<Parcel, bool> conditionDelegate = null)
         {
                List<Parcel> parcelList = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelsPath);
                var v = from item in parcelList
                        select item; 
                if (conditionDelegate == null)
                    return v.AsEnumerable().OrderByDescending(p => p.CodeParcel);
                return v.Where(conditionDelegate).OrderByDescending(p => p.CodeParcel);
        }

        public IEnumerable<Drone> GetDronesByCondition(Func<Drone, bool> conditionDelegate = null)
        {
            List<Drone> droneList = XMLTools.LoadListFromXMLSerializer<Drone>(dronesPath);
            var v = from item in droneList
                    select item; //item.Clone();
            if (conditionDelegate == null)
                return v.AsEnumerable().OrderByDescending(d => d.CodeDrone);
            return v.Where(conditionDelegate).OrderByDescending(d => d.CodeDrone);
        }

        public IEnumerable<Customer> GetCustomersByCondition(Func<Customer, bool> conditionDelegate = null)
        {
            List<Customer> customerList = XMLTools.LoadListFromXMLSerializer<Customer>(customersPath);
            var v = from item in customerList
                    select item; //item.Clone();
            if (conditionDelegate == null)
                return v.AsEnumerable().OrderByDescending(c => c.IdCustomer);
            return v.Where(conditionDelegate).OrderByDescending(c => c.IdCustomer);
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