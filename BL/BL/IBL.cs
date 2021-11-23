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
        void addBaseStation(BaseStation b);
        void addDrone(Drone d);
        void addCustomer(Customer c);
        void addParcel(Parcel p);
        void updateDrone(int id, string m);
        void updateBaseStation(int id, string name="",int numOfChargePositions=0);
        void updateCustomer(int id, string name = "", int phone = 0);
        void updateSendingDroneToCharge(int id);
        void updateReleasingDroneFromCharge(int id, double timeOfCharging);
        void updateParcelToDrone(int idD);
        void updateParcelPickedUpByDrone(int idD);
        void updateDeliveredParcelByDrone(int idD);
        BaseStation getBaseStation(int idS);
        Drone getDrone(int idD);
        Customer getCustomer(int idC);
        Parcel getParcel(int idP);
        IEnumerable<BaseStationList> getAllBaseStations();
        IEnumerable<DroneList> getAllDrones();
        IEnumerable<CustomerList> getAllCustomers();
        IEnumerable<ParcelList> getAllParcels();
        IEnumerable<ParcelList> getAllParcelsWithoutDrone();
        IEnumerable<BaseStationList> getAllBaseStationsWithChargePositions();
    }
}
