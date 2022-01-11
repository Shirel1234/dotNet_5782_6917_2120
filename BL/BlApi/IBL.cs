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
        void AddBaseStation(BaseStation b);
        void AddDrone(Drone d, int idStation);
        void AddCustomer(Customer c);
        void AddParcel(Parcel p);
        void UpdateDrone(int id, string model);
        void UpdateBaseStation(int id, int name, int numOfChargeSlots);
        void UpdateCustomer(int id, string name, string phone,bool isWorker);
        void UpdateSendingDroneToCharge(int id);
        void UpdateReleasingDroneFromCharge(int id);
        void UpdateParcelToDrone(int idD);
        void UpdateParcelPickedUpByDrone(int idD);
        void UpdateDeliveredParcelByDrone(int idD);
        BaseStation GetBaseStation(int idS);
        Drone GetDrone(int idD);
        Customer GetCustomer(int idC);
        Parcel GetParcel(int idP);
        IEnumerable<BaseStationForList> GetBaseStations();
        IEnumerable<DroneForList> GetDrones();
        IEnumerable<CustomerForList> GetCustomers();
        IEnumerable<ParcelForList> GetParcels();
        IEnumerable<ParcelForList> GetAllParcelsWithoutDrone();
        IEnumerable<BaseStationForList> GetAllBaseStationsWithChargePositions();
        IEnumerable<DroneForList> GetDronesByStatus(int num);
        IEnumerable<DroneForList> GetDronesByWeight(int num);
        void RemoveParcel(int id);
        void StartSimulator(int id, Action updateDelegate, Func<bool> stopDelegate);

    }
}
