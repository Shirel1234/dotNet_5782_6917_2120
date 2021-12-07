﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL
{
    public partial class BLObject
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
            if (c.Location.Longitude < -180 || c.Location.Longitude > 180)
                throw new AddingProblemException("The longitude must be between -180 to 180");
            if (c.Location.Latitude < -90 || c.Location.Latitude > 90)
                throw new AddingProblemException("The latitude must be between -90 to 90");
            try
            {
                IDAL.DO.Customer doCustomer = new IDAL.DO.Customer()
                {
                    IdCustomer = c.Id,
                    NameCustomer = c.Name,
                    Phone = c.Phone,
                    Longitude = c.Location.Longitude,
                    Latitude = c.Location.Latitude,
                };
                dl.AddCustomer(doCustomer);
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
            IDAL.DO.Customer tempC = dl.GetCustomer(id);
            if (!name.Equals("0"))
                tempC.NameCustomer = name;
            if (!phone.Equals("0"))
                tempC.Phone = phone;
            try
            {
                dl.UpDateCustomer(tempC);
            }
            catch (Exception)
            {
                throw new UpdateProblemException();
            }
        }
        /// <summary>
        /// the function moves on the parcel and create list of all the parcels that this customer send
        /// </summary>
        /// <param name="id"> id of customer</param>
        /// <returns>list of parcelsthat this customer send</returns>
        public IEnumerable<ParcelCustomer> GetSendParcels(int id)
        {
            IEnumerable<ParcelCustomer> sendParcels = from parcel in dl.GetParcels()
                                                      where parcel.SenderId == id
                                                      select new ParcelCustomer
                                                      {
                                                          Id = parcel.CodeParcel,
                                                          Priority = (Priorities)parcel.Priority,
                                                          CustomerId = id,
                                                          Weight = (WeightCategories)parcel.Weight,
                                                          Status = (ParcelStatuses)GetParcelStatus(parcel)

                                                      };
            return sendParcels;
        }
        /// <summary>
        ///  the function moves on the parcels and creates a list of all the parcels that this customer got
        /// </summary>
        /// <param name="id">id of customer</param>
        /// <returns>list of parcels that this customer got</returns>
        public IEnumerable<ParcelCustomer> GetTargetParcel(int id)
        {
            IEnumerable<ParcelCustomer> targetParcel = from parcel in dl.GetParcels()
                                                      where parcel.TargetId == id
                                                      select new ParcelCustomer
                                                      {
                                                          Id = parcel.CodeParcel,
                                                          Priority = (Priorities)parcel.Priority,
                                                          CustomerId = id,
                                                          Weight = (WeightCategories)parcel.Weight,
                                                          Status= (ParcelStatuses)GetParcelStatus(parcel)
                                                      };
            return targetParcel;
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
                IDAL.DO.Customer dalCustomer = dl.GetCustomer(id);
                return new BO.Customer()
                {
                    Id = dalCustomer.IdCustomer,
                    Name = dalCustomer.NameCustomer,
                    Phone = dalCustomer.Phone,
                    Location = new Location(dalCustomer.Longitude,dalCustomer.Latitude ),
                    SendParcels = GetSendParcels(id),
                    TargetParcels =GetTargetParcel(id)

                };
            }
            catch (IDAL.DO.DoesntExistException customerException)
            {
                throw new GetDetailsProblemException($"Customer id {id} was not found", customerException);
            }
        }
        public IEnumerable<CustomerList> GetAllCustomers()
        {
            List<IDAL.DO.Customer> DOcustomersList = dl.GetCustomers().ToList();
            List<IDAL.DO.Parcel> DOparcelsList = dl.GetParcels().ToList();
            return
                from customer in DOcustomersList
                let numOfDeliveredParcels = DOparcelsList.Count(parcel => parcel.SenderId == customer.IdCustomer && DateTime.Compare(parcel.Delivered, DateTime.Now) <= 0)
                let numOfNotDeliveredParcels= DOparcelsList.Count(parcel => parcel.SenderId == customer.IdCustomer && DateTime.Compare(parcel.Delivered, DateTime.Now) > 0
                && DateTime.Compare(parcel.PickedUp, DateTime.Now) > 0 && DateTime.Compare(parcel.Scheduled, DateTime.Now) <= 0)
                let numOfParcelsInWay= DOparcelsList.Count(parcel => parcel.SenderId == customer.IdCustomer && DateTime.Compare(parcel.Delivered, DateTime.Now) > 0
                && DateTime.Compare(parcel.PickedUp, DateTime.Now) <= 0)
                let numOfAcceptedParcels= DOparcelsList.Count(parcel => parcel.TargetId == customer.IdCustomer && DateTime.Compare(parcel.Delivered, DateTime.Now) <= 0)
                select new CustomerList()
                {
                    Id=customer.IdCustomer,
                    Name=customer.NameCustomer,
                    Phone=customer.Phone,
                    CountDeliveredParcels= numOfDeliveredParcels,
                    CountNotDeliveredParcels= numOfNotDeliveredParcels,
                    CountParcelsInWay= numOfParcelsInWay,
                    CountAcceptedParcelsByCustomer= numOfAcceptedParcels
                };
        }

    }
}
