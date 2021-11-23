using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;
using IBL.BO;

namespace IBL
{
    public interface IBL
    {
        void AddBaseStation(BaseStation b);
        void AddDrone(Drone d);
        void AddCustomer(Customer c);
        void AddParcel(Parcel p);
        void UpdateDrone(int id, string m);
        void UpdateBaseStation(int id, string name="",int numOfChargePositions=0);
        void UpdateCustomer(int id, string name = "", int phone = 0);
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
