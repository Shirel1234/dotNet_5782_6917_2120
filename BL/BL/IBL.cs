using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    public interface IBl
    {
        void AddBaseStation(BaseStation b);
         void AddDrone(Drone d, int idStation);
        void AddCustomer(Customer c);
         void AddParcel(Parcel p);
        void UpdateDrone(int id, string model);
        void UpdateBaseStation(int id, int name,int numOfChargePositions);
        void UpdateCustomer(int id, string name, string phone);
        void UpdateSendingDroneToCharge(int id);
        void UpdateReleasingDroneFromCharge(int id, double timeOfCharging);
        void UpdateParcelToDrone(int idD);
        void UpdateParcelPickedUpByDrone(int idD);
        void UpdateDeliveredParcelByDrone(int idD);
        BaseStation GetBaseStation(int idS);
        Drone GetDrone(int idD);
        Customer GetCustomer(int idC);
        Parcel GetParcel(int idP);
        IEnumerable<BaseStationList> GetAllBaseStations();
        IEnumerable<DroneList> GetAllDrones();
        IEnumerable<CustomerList> GetAllCustomers();
        IEnumerable<ParcelList> GetAllParcels();
        IEnumerable<ParcelList> GetAllParcelsWithoutDrone();
        IEnumerable<BaseStationList> GetAllBaseStationsWithChargePositions();
    }
}
