using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalObject;
using IDAL.DO;

namespace DAL
{
    namespace IDAL
    {
        interface IDal
        {
            void AddStation(BaseStation b);
            void AddDrone(Drone d);
            void AddCustomer(Customer c);
            void AddParcel(Parcel p);
            void UpdateParcelToDrone(int idP, int idD);
            void UpdatePickedUp(int idP);
            void UpdateDeliver(int idP);
            void UpDateCharge(int idD, int idS);
            void ReleaseCharge(int idD, int idS);
            BaseStation ShowStation(int idS);
            Drone ShowDrone(int idD);
            Customer ShowCustomer(int idC);
            Parcel ShowParcel(int idP);
            IEnumerable<BaseStation> ShowListBaseStations();
            IEnumerable<Drone> ShowListDrones();
            IEnumerable<Customer> ShowListCustomers();
            IEnumerable<Parcel> ShowListParcels();
            IEnumerable<Parcel> ListParcelWithoutDrone();
            IEnumerable<BaseStation> ListBaseStationsSlots();
            double[] AskElectrical(Drone d);
       
        }
    }

}
