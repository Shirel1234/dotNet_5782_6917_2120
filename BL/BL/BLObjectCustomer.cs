using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL
{
    public partial class BLObject
    {
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
        public void UpdateCustomer(int id, string name, string phone)
        {
            if (id < 100000000 || id > 999999999)
                throw new UpdateProblemException("The customer ID number must contain 9 digits");
            IDAL.DO.Customer tempC = dl.GetCustomer(id);
            if (!name.Equals(""))
                tempC.NameCustomer = name;
            if (!phone.Equals(""))
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
        public int GetParcelStatus(IDAL.DO.Parcel parcel)
        {
            DateTime temp = new DateTime(0);
            if (parcel.Scheduled == temp)
                return 0;
            if (parcel.PickedUp == temp)
                return 1;
            if (parcel.Delivered == temp)
                return 2;
            return 3;
        }
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
