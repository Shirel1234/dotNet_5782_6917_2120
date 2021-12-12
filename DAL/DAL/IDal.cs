﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalObject;


    namespace IDAL.DO
    {

        public interface IDal
        {
            void AddStation(BaseStation b);
            void AddDrone(Drone d);
            void AddCustomer(Customer c);
            void AddParcel(Parcel p);
            void AddDroneCharge(int idD, int idS);
            void RemoveDroneCharge(DroneCharge dc);
            void UpDateBaseStation(BaseStation b);
            void UpDateParcel(Parcel p);
            void UpDateDrone(Drone d);
            void UpDateCustomer(Customer c);
            BaseStation GetStation(int idS);
            Drone GetDrone(int idD);
            Customer GetCustomer(int idC);
            Parcel GetParcel(int idP);
            DroneCharge GetDroneCharge(int idD);
            IEnumerable<BaseStation> GetStationsByCondition(Func<BaseStation, bool> conditionDelegate = null);
            IEnumerable<Parcel> GetParcelsByCondition(Func<Parcel, bool> conditionDelegate = null);
            IEnumerable<Drone> GetDronesByCondition(Func<Drone, bool> conditionDelegate = null);
            IEnumerable<Customer> GetCustomersByCondition(Func<Customer, bool> conditionDelegate = null);
            double[] AskElectrical();

        }
    }


