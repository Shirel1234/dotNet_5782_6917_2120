using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL
{
    //שינוי אחרון בהצלחה
    public partial class BLObject : IBl
    {
        public double free;
        public double easy;
        public double medium;
        public double heavy;
        public double chargingRate;

        #region
        IDAL.DO.IDal dl;
        List<DroneList> BODrones;
        public BLObject()
        {
            dl = new DalObject.DalObject();
            double[] arr = dl.AskElectrical();
            free = arr[0];
            easy = arr[1];
            medium = arr[2];
            heavy = arr[3];
            chargingRate = arr[4];
            IEnumerable< DroneList> BODrones = from IDAL.DO.Drone item in dl.GetDrones().ToList()
                       select new DroneList()
                       {
                           Id = item.CodeDrone,
                           ModelDrone = item.ModelDrone,
                           Weight = (WeightCategories)item.MaxWeight,
                          // Battery =
                           };
        }
        public void UpdateSendingDroneToCharge(int id)
        {
            DroneList droneList = BODrones.Find(droneL => droneL.Id == id);
            if (droneList.Id == 0)
                throw new UpdateProblemException("This drone doesn't exist");
            if (droneList.DroneStatus == (DroneStatuses)0)
            {
                double minDistance = 10000000;
                IDAL.DO.BaseStation closeStation = new IDAL.DO.BaseStation();
                foreach (var item in dl.GetBaseStations())
                {
                    double distance = HelpClass.GetDistance(new Location(item.Longitude, item.Latitude), droneList.LocationNow);
                    if (minDistance > distance)
                    {
                        minDistance = distance;
                        closeStation = item;
                    }
                }
                //sending the drone to charging
                if (droneList.Battery >= minDistance * free && closeStation.ChargeSlots > 0)
                {
                    //update drone
                    BODrones.Remove(droneList);
                    droneList.Battery -= minDistance * free;
                    droneList.LocationNow.Longitude = closeStation.Longitude;
                    droneList.LocationNow.Latitude = closeStation.Latitude;
                    droneList.DroneStatus = (DroneStatuses)1;
                    BODrones.Add(droneList);
                    //update station
                    dl.GetBaseStations().ToList().Remove(closeStation);
                    closeStation.ChargeSlots--;
                    dl.GetBaseStations().ToList().Add(closeStation);
                    //update drone Charge
                    DroneCharge droneCharge = new DroneCharge() { Battery = droneList.Battery, Id = droneList.Id };
                }
                else
                    throw new UpdateProblemException("It is impossible to send this drone to charging");
            }
            else
                throw new UpdateProblemException("The drone isn't free for charging");
        }
        public void UpdateReleasingDroneFromCharge(int id, double timeOfCharging)
        {
            DroneList droneList = BODrones.Find(droneL => droneL.Id == id);
            if (droneList.Id == 0)
                throw new UpdateProblemException("This drone doesn't exist");
            if (droneList.DroneStatus == (DroneStatuses)1)
            {
                //update drone
                BODrones.Remove(droneList);
                droneList.Battery = timeOfCharging * chargingRate;
                droneList.DroneStatus = (DroneStatuses)0;
                BODrones.Add(droneList);
                //update station
                IDAL.DO.BaseStation myStation = dl.GetBaseStations().ToList().Find(station => station.Longitude == droneList.LocationNow.Longitude && station.Latitude == droneList.LocationNow.Latitude);
                dl.GetBaseStations().ToList().Remove(myStation);
                myStation.ChargeSlots++;
                dl.GetBaseStations().ToList().Add(myStation);
            }
                
        }
        public void UpdateParcelToDrone(int id)
        {
    //        DroneList droneList = BODrones.Find(droneL => droneL.Id == id);
    //        if (droneList.Id == 0)
    //            throw new UpdateProblemException("This drone doesn't exist");
    //        if (droneList.DroneStatus == (DroneStatuses)0)
    //        {
    //            var groups = dl.GetParcels().ToList().GroupBy(parcel => parcel.Priority);
    //            List<IDAL.DO.Parcel> g0=new List<IDAL.DO.Parcel>();
    //            List<IDAL.DO.Parcel> g1=new List<IDAL.DO.Parcel>();
    //            List<IDAL.DO.Parcel> g2=new List<IDAL.DO.Parcel>();
    //            foreach (IGrouping<Priorities, IDAL.DO.Parcel> group in groups)
    //            {
    //                if (group.Key == (Priorities)0)
    //                    g0 = group.ToList();
    //                else
    //                    if (group.Key == (Priorities)1)
    //                    g1 = group.ToList();
    //                else
    //                    g2 = group.ToList();
    //            }
    //            var matchWeightParcels = from item in g2
    //                                     where ((int)item.Weight)<= ((int)droneList.Weight)
    //                                     select item;
    //            if(matchWeightParcels.ToList().Count!=0)
    //            {
    //                IDAL.DO.Parcel closerParcel=new IDAL.DO.Parcel();
    //                double minDistance=10000000;
    //                foreach (var item in matchWeightParcels)
    //                {
    //                    IDAL.DO.Customer customer = dl.GetCustomer(item.SenderId);
    //                    double distance = HelpClass.GetDistance(new Location(customer.Longitude, customer.Latitude), droneList.LocationNow);
    //                    if (minDistance > distance)
    //                    {
    //                        minDistance = distance;
    //                        closerParcel = item;
    //                    }

    //                } 
    //                if()
                                     
    //            }


    //        }


    }
        public void UpdateParcelPickedUpByDrone(int idD)
        {

        }
        public void UpdateDeliveredParcelByDrone(int idD)
        {

        }
        public IEnumerable<ParcelList> GetAllParcelsWithoutDrone()
        {
            IEnumerable<ParcelList> p = new List<ParcelList>();
            return p;
        }
        public IEnumerable<BaseStationList> GetAllBaseStationsWithChargePositions()
        {
            IEnumerable<BaseStationList> p = new List<BaseStationList>();
            return p;
        }
        #endregion
    }
}
