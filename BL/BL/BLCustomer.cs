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
        /// the function checks the details and throw errors in step and then adds a new castumer to the list of customers
        /// </summary>
        /// <param name="c"> a new castumer</param>
        public void AddCustomer(Customer c)
        {
            if (c.Id < 100000000 || c.Id > 999999999)
                throw new AddingProblemException("The customer ID number must contain 9 digits");
            if (c.Phone.Length != 10)
                throw new AddingProblemException("The phone number isn't valid");
            if (c.Location.Longitude < 29.3 || c.Location.Longitude > 33.7)
                throw new AddingProblemException("The longitude must be between 29.3 to 33.7");
            if (c.Location.Latitude < 33.5 || c.Location.Latitude > 36.3)
                throw new AddingProblemException("The latitude must be between 33.5 to 36.3");
            try
            {
                DO.Customer doCustomer = new DO.Customer()
                {
                    IdCustomer = c.Id,
                    NameCustomer = c.Name,
                    Phone = c.Phone,
                    Longitude = c.Location.Longitude,
                    Latitude = c.Location.Latitude,
                };
                dal.AddCustomer(doCustomer);
            }

            catch (Exception ex)
            {
                throw new AddingProblemException("Can't add this customer", ex);
            }
        }
        /// <summary>
        /// the funcrion gets new details of customer, checks them and throw matching errors
        /// In addition it calls to tha function that update the customer from dal with the new details
        /// </summary>
        /// <param name="id"> id of customer</param>
        /// <param name="name"> name of customer</param>
        /// <param name="phone"> phone of customer</param>
        public void UpdateCustomer(int id, string name, string phone)
        {
            if (id < 100000000 || id > 999999999)
                throw new UpdateProblemException("The customer ID number must contain 9 digits");
            try
            {
                DO.Customer tempC = dal.GetCustomer(id);
                if (!name.Equals("0"))
                    tempC.NameCustomer = name;
                if (!phone.Equals("0"))
                    tempC.Phone = phone;
                dal.UpDateCustomer(tempC);
            }
            catch (Exception ex)
            {
                throw new UpdateProblemException(ex.Message, ex);
            }
        }
        /// <summary>
        /// the function moves on the parcel and create list of all the parcels that this customer send
        /// </summary>
        /// <param name="id"> id of customer</param>
        /// <returns>list of parcelsthat this customer send</returns>
        public IEnumerable<ParcelInCustomer> GetSendParcels(int id)
        {
            try
            {
                IEnumerable<ParcelInCustomer> sendParcels = from parcel in dal.GetParcelsByCondition()
                                                          where parcel.SenderId == id
                                                          select new ParcelInCustomer
                                                          {
                                                              Id = parcel.CodeParcel,
                                                              Priority = (Priorities)parcel.Priority,
                                                              SecondSideCustomer = new CustomerInParcel() { Id = parcel.TargetId, Name = dal.GetCustomer(parcel.TargetId).NameCustomer },
                                                              Weight = (WeightCategories)parcel.Weight,
                                                              Status = (ParcelStatuses)GetParcelStatus(parcel)
                                                          };
                return sendParcels;
            }
            catch
            {
                throw new GetDetailsProblemException();
            }
        }
        /// <summary>
        ///  the function moves on the parcels and creates a list of all the parcels that this customer got
        /// </summary>
        /// <param name="id">id of customer</param>
        /// <returns>list of parcels that this customer got</returns>
        public IEnumerable<ParcelInCustomer> GetTargetParcel(int id)
        {
            try
            {
                IEnumerable<ParcelInCustomer> sendParcels = from parcel in dal.GetParcelsByCondition()
                                                          where parcel.SenderId == id
                                                          select new ParcelInCustomer
                                                          {
                                                              Id = parcel.CodeParcel,
                                                              Priority = (Priorities)parcel.Priority,
                                                              SecondSideCustomer = new CustomerInParcel() { Id = parcel.TargetId, Name = dal.GetCustomer(parcel.TargetId).NameCustomer },
                                                              Weight = (WeightCategories)parcel.Weight,
                                                              Status = (ParcelStatuses)GetParcelStatus(parcel)
                                                          };
                return sendParcels;
            }
            catch
            {
                throw new GetDetailsProblemException();
            }
        }
        /// <summary>
        /// the function bring from the dal the details of the customer and create by it a customer of bl
        /// </summary>
        /// <param name="id"> id of customer</param>
        /// <returns>a customer has this id</returns>
        public Customer GetCustomer(int id)
        {
            try
            {
                DO.Customer dalCustomer = dal.GetCustomer(id);
                return new BO.Customer()
                {
                    Id = dalCustomer.IdCustomer,
                    Name = dalCustomer.NameCustomer,
                    Phone = dalCustomer.Phone,
                    Location = new Location(dalCustomer.Longitude, dalCustomer.Latitude),
                    SendParcels = GetSendParcels(id),
                    TargetParcels = GetTargetParcel(id)

                };
            }
            catch (Exception ex)
            {
                throw new GetDetailsProblemException(ex.Message, ex);
                //throw new GetDetailsProblemException($"Customer id {id} was not found", ex);
            }
        }
        /// <summary>
        /// the function brings all of the customers from dal and add the mess detail for bl Customer
        /// </summary>
        /// <returns>list of customer</returns>
        public IEnumerable<CustomerForList> GetCustomers()
        {
            List<DO.Customer> DOcustomersList = dal.GetCustomersByCondition().ToList();
            List<DO.Parcel> DOparcelsList = dal.GetParcelsByCondition().ToList();
            return
                from customer in DOcustomersList
                let numOfDeliveredParcels = DOparcelsList.Count(parcel => parcel.SenderId == customer.IdCustomer && parcel.Delivered != null)
                let numOfNotDeliveredParcels = DOparcelsList.Count(parcel => parcel.SenderId == customer.IdCustomer && parcel.Delivered == null)
                let numOfParcelsInWay = DOparcelsList.Count(parcel => parcel.SenderId == customer.IdCustomer && parcel.PickedUp == null)
                let numOfAcceptedParcels = DOparcelsList.Count(parcel => parcel.TargetId == customer.IdCustomer && parcel.Requested != null)
                select new CustomerForList()
                {
                    Id = customer.IdCustomer,
                    Name = customer.NameCustomer,
                    Phone = customer.Phone,
                    CountDeliveredParcels = numOfDeliveredParcels,
                    CountNotDeliveredParcels = numOfNotDeliveredParcels,
                    CountParcelsInWay = numOfParcelsInWay,
                    CountAcceptedParcelsByCustomer = numOfAcceptedParcels
                };
        }

    }
}

