using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BL
{
    public partial class BL
    {
        /// <summary>
        /// the function checks the details and throw errors in step and then adds a new parcel to the list of parcels
        /// </summary>
        /// <param name="c"> a new parcel</param>
        public void AddParcel(Parcel p)
        {
            if (p.SenderCustomer.Id < 100000000 || p.SenderCustomer.Id > 999999999)
                throw new AddingProblemException("The id of sender must contain 9 digits");
            if (p.TargetCustomer.Id < 100000000 || p.TargetCustomer.Id > 999999999)
                throw new AddingProblemException("The id of target must contain 9 digits");
            if (p.Weight < 0)///operator>
                throw new AddingProblemException("The weight isn't valid");
            if (p.Priority < 0)///operator>
                throw new AddingProblemException("The priority isn't valid");
            p.Requested = DateTime.Now;
            p.Scheduled = null;
            p.PickedUp = null;
            p.Delivered = null;
            p.DroneInParcel = new DroneInParcel();
            try
            {
                DO.Parcel doParcel = new DO.Parcel()
                {
                    SenderId = p.SenderCustomer.Id,
                    TargetId = p.TargetCustomer.Id,
                    Weight = (DO.WeightCategories)p.Weight,
                    DroneId = p.DroneInParcel.Id,
                    Priority = (DO.Priorities)p.Priority,
                    Delivered = p.Delivered,
                    Requested = p.Requested,
                    PickedUp = p.PickedUp,
                    Scheduled = p.Scheduled,
                };
                dal.AddParcel(doParcel);
            }

            catch (Exception ex)
            {
                throw new AddingProblemException(ex.Message, ex);
            }
        }
        /// <summary>
        /// the function brings from the dal the details of the parcel and creates by it a parcel of bl
        /// </summary>
        /// <param name="id">id of parcel</param>
        /// <returns>a parcel</returns>
        public Parcel GetParcel(int id)
        {
            try
            {
                DO.Parcel dalParcel = dal.GetParcel(id);
                DroneForList d= BODrones.Find(drone => drone.Id == dalParcel.DroneId);
                DroneInParcel droneInParcel = new DroneInParcel();
                if (d != null)
                {
                    droneInParcel.Id = dalParcel.DroneId;
                    droneInParcel.Battery = d.Battery;
                    droneInParcel.LocationNow = d.LocationNow;
                }
                return new Parcel()
                {
                    CodeParcel = dalParcel.CodeParcel,
                    SenderCustomer = new CustomerInParcel() { Id = dalParcel.SenderId, Name = dal.GetCustomer(dalParcel.SenderId).NameCustomer },
                    TargetCustomer = new CustomerInParcel() { Id = dalParcel.TargetId, Name = dal.GetCustomer(dalParcel.TargetId).NameCustomer },
                    DroneInParcel = droneInParcel,
                    Weight = (WeightCategories)dalParcel.Weight,
                    Priority = (Priorities)dalParcel.Priority,
                    Delivered = dalParcel.Delivered,
                    Requested = dalParcel.Requested,
                    PickedUp = dalParcel.PickedUp,
                    Scheduled = dalParcel.Scheduled
                };
            }
            catch (Exception ex)
            {
                throw new GetDetailsProblemException(ex.Message, ex);
            }
        }
        /// <summary>
        /// the function brings all the parcels from the dal and add the mess details for the bl parcel
        /// </summary>
        /// <returns>list of parcels</returns>
        public IEnumerable<ParcelForList> GetParcels()
        {
            try
            {
                List<DO.Parcel> DOparcelsList = dal.GetParcelsByCondition().ToList();
                List<DO.Customer> DOcustomersList = dal.GetCustomersByCondition().ToList();
                return
                    from parcel in DOparcelsList
                    let senderName = DOcustomersList.Find(customer => customer.IdCustomer == parcel.SenderId).NameCustomer
                    let receiverName = DOcustomersList.Find(customer => customer.IdCustomer == parcel.TargetId).NameCustomer
                    let parcelStatus = GetParcelStatus(parcel)
                    select new ParcelForList()
                    {
                        Id = parcel.CodeParcel,
                        NameSender = senderName,
                        NameTarget = receiverName,
                        Weight = (WeightCategories)parcel.Weight,
                        Priority = (Priorities)parcel.Priority,
                        Status = parcelStatus
                    };
            }
            catch (Exception ex)
            {
                throw new GetDetailsProblemException(ex.Message, ex);
            }
        }
        /// <summary>
        /// the function move on the dal parcels and selects the parcel that are without drone and create from them a list
        /// </summary>
        /// <returns>this list of parcels witthout drone</returns>
        public IEnumerable<ParcelForList> GetAllParcelsWithoutDrone()
        {
            try
            {
                List<DO.Parcel> DOparcelsWithoutDroneList = dal.GetParcelsByCondition(parcel => parcel.DroneId == 0).ToList();
                List<DO.Customer> DOcustomersList = dal.GetCustomersByCondition().ToList();
                return
                    from parcel in DOparcelsWithoutDroneList
                    let senderName = DOcustomersList.Find(customer => customer.IdCustomer == parcel.SenderId).NameCustomer
                    let receiverName = DOcustomersList.Find(customer => customer.IdCustomer == parcel.TargetId).NameCustomer
                    let parcelStatus = ParcelStatuses.requested
                    select new ParcelForList()
                    {
                        Id = parcel.CodeParcel,
                        NameSender = senderName,
                        NameTarget = receiverName,
                        Weight = (WeightCategories)parcel.Weight,
                        Priority = (Priorities)parcel.Priority,
                        Status = parcelStatus
                    };
            }
            catch (Exception ex)
            {
                throw new GetDetailsProblemException(ex.Message, ex);
            }
        }
        public void RemoveParcel(int id)
        {
            dal.RemoveParcel(id);
        }

    }
}
