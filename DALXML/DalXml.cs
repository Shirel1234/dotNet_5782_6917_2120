using System;
using System.Collections.Generic;
using DalApi;
using DO;

namespace Dal
{
    sealed class DalXml : IDal
    {
        static readonly IDal instance = new DalXml();
        public static IDal Instance { get => instance; }
        DalXml() { }

        public void AddStation(BaseStation b)
        {
            throw new NotImplementedException();
        }

        public void AddDrone(Drone d)
        {
            throw new NotImplementedException();
        }

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

        public void UpDateBaseStation(BaseStation b)
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

        public BaseStation GetStation(int idS)
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

        public IEnumerable<BaseStation> GetStationsByCondition(Func<BaseStation, bool> conditionDelegate = null)
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
    }
}
