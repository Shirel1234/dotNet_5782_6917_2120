using System;
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
            void UpdateParcelToDrone(int idP, int idD);
            void UpdatePickedUp(int idP);
            void UpdateDeliver(int idP);
            void UpDateCharge(int idD, int idS);
            void ReleaseCharge(int idD, int idS);
            BaseStation GetStation(int idS);
            Drone GetDrone(int idD);
            Customer GetCustomer(int idC);
            Parcel GetParcel(int idP);
            IEnumerable<BaseStation> GetBaseStations();
            IEnumerable<Drone> GetDrones();
            IEnumerable<Customer> GetCustomers();
            IEnumerable<Parcel> GetParcels();
            IEnumerable<Parcel> ListParcelWithoutDrone();
            IEnumerable<BaseStation> ListBaseStationsSlots();
            double[] AskElectrical(Drone d);
       
       }
}


