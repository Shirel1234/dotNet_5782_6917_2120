using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace BL
{
   public partial class BLObject
    {
        IDAL.DO.IDal dl;
        public BLObject()
        {
            dl = new DalObject.DalObject();
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
                    Taretparcels= new List<ParcelCustomer>()

                };
            }
            catch(IDAL.DO.DoesntExistException customerException)
            {
                throw new GetDetailsProblemException($"Customer id {id} was not found", customerException);
            }
        }

    }
}
