using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;


namespace BlApi
{
    public interface IBL
    {
        #region adding
        void AddBaseStation(BaseStation b);
        void AddDrone(Drone d, int idStation);
        void AddCustomer(Customer c);
        void AddParcel(Parcel p);
        #endregion
        #region updating
        void UpdateDrone(Drone d);
        void UpdateBaseStation(int id, int name, int numOfChargeSlots);
        void UpdateCustomer(int id, string name, string phone,bool isWorker);
        void UpdateSendingDroneToCharge(int id);
        void UpdateReleasingDroneFromCharge(int id);
        bool UpdateParcelToDrone(int idD);
        void UpdateParcelPickedUpByDrone(int idD);
        void UpdateDeliveredParcelByDrone(int idD);
        #endregion
        #region getting single object
        BaseStation GetBaseStation(int idS);
        Drone GetDrone(int idD);
        Customer GetCustomer(int idC);
        Parcel GetParcel(int idP);
        #endregion
        #region getting lists of objects
        IEnumerable<BaseStationForList> GetBaseStations();
        IEnumerable<DroneForList> GetDrones();
        IEnumerable<CustomerForList> GetCustomers();
        IEnumerable<ParcelForList> GetParcels();
        IEnumerable<ParcelForList> GetAllParcelsWithoutDrone();
        IEnumerable<BaseStationForList> GetAllBaseStationsWithChargePositions();
        IEnumerable<DroneForList> GetDronesByStatus(int num);
        IEnumerable<DroneForList> GetDronesByWeight(int num);
        #endregion
        #region removing
        void RemoveParcel(int id);
        #endregion
        #region simulator function
        void StartSimulator(int id, Action updateDelegate, Func<bool> stopDelegate);
        #endregion
    }
}
