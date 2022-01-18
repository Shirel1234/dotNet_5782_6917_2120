using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using DO;

namespace DalApi
{
    public interface IDal
    {
        #region adding
        void AddStation(BaseStation b);
        void AddDrone(Drone d);
        void AddCustomer(Customer c);
        void AddParcel(Parcel p);
        void AddDroneCharge(int idD, int idS, DateTime? dateTimeBegining);
        #endregion
        #region removing
        void RemoveDroneCharge(DroneCharge dc);
        void RemoveParcel(int id);
        #endregion
        #region updating
        void UpDateBaseStation(BaseStation b);
        void UpDateParcel(Parcel p);
        void UpDateDrone(Drone d);
        void UpDateCustomer(Customer c);
        #endregion
        #region getting single
        BaseStation GetStation(int idS);
        Drone GetDrone(int idD);
        Customer GetCustomer(int idC);
        Parcel GetParcel(int idP);
        DroneCharge GetDroneCharge(int idD);
        #endregion
        #region getting all
        IEnumerable<BaseStation> GetStationsByCondition(Func<BaseStation, bool> conditionDelegate = null);
        IEnumerable<Parcel> GetParcelsByCondition(Func<Parcel, bool> conditionDelegate = null);
        IEnumerable<Drone> GetDronesByCondition(Func<Drone, bool> conditionDelegate = null);
        IEnumerable<Customer> GetCustomersByCondition(Func<Customer, bool> conditionDelegate = null);
        IEnumerable<DroneCharge> GetDronesChargeByCondition(Func<DroneCharge, bool> conditionDelegate = null);
        #endregion
        double[] AskElectrical();
    }
}
