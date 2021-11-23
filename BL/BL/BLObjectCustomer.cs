using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL
{
    public partial class BLObjectCustomer
    {
        public void AddCustomer(Customer c)
        {
            if (c.Id < 100000000 || c.Id > 999999999)
                throw new AddingProblemException("The ID number must contain 9 digits");
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
            public Customer GetCustomer(int id)
            {
            Customer customer = default;
            try
            {
                IDAL.DO.Customer dalCustomer = dl.GetCustomer(id);
                return new Customer()
                {
                    Id = dalCustomer.IdCustomer,
                    Name = dalCustomer.NameCustomer,
                    Phone = dalCustomer.Phone,
                    //Location 
                    SendParcels = new List<ParcelCustomer>(),
                    Taretparcels = new List<ParcelCustomer>()

                };
            }
            catch (IDAL.DO.DoesntExistException customerException)
            {
                throw new GetDetailsProblemException($"Customer id {id} was not found", customerException);
            }
        }
    }
}
