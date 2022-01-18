using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace Dal
{
    partial class DalObject
    {
        #region
        /// <summary>
        /// the function check if this station is exist yet
        /// </summary>
        /// <param name="id"></param>
        /// <returns> true if it is exist and else false</returns>
        public bool CheckCustomer(int id)
        {
            return DataSource.customers.Any(customer => customer.IdCustomer == id);
        }
        /// <summary>
        /// the function gets an object of customer and adds it to the list of customers
        /// </summary>
        /// <param name="c"> a customer</param>
        public void AddCustomer(Customer c)
        {
            if (CheckCustomer(c.IdCustomer))
                throw new AlreadyExistException("The customer already exist in the system");
            DataSource.customers.Add(c);
        }
        /// <summary>
        /// the function update every field to the new details
        /// </summary>
        /// <param name="c"> a new customer</param>
        public void UpDateCustomer(Customer c)
        {
            Customer myCustomer = DataSource.customers.Find(x => x.IdCustomer == c.IdCustomer);
            if (myCustomer.IdCustomer != c.IdCustomer)
                throw new DoesntExistException("This customer doesn't exist in the system");
            myCustomer.IdCustomer = c.IdCustomer;
            myCustomer.NameCustomer = c.NameCustomer;
            myCustomer.Phone = c.Phone;
            myCustomer.Longitude = c.Longitude;
            myCustomer.Latitude = c.Latitude;
            myCustomer.IsWorker = c.IsWorker;
            DataSource.customers.Remove(c);
            DataSource.customers.Add(myCustomer);
        }
        /// <summary>
        /// the function searchs the customer with the id that it got
        /// </summary>
        /// <param name="id"></param>
        /// <returns>the details of this customer</returns>
        public Customer GetCustomer(int id)
        {
            Customer c = DataSource.customers.Find(customer => customer.IdCustomer == id);
            if (c.IdCustomer == id)
                return c;
            throw new DoesntExistException("This customer does not exist");
        }
        /// <summary>
        /// the function show the list of customers
        /// </summary>
        /// <returns>list of customers</returns>
        public IEnumerable<Customer> GetCustomersByCondition(Func<Customer, bool> conditionDelegate = null)
        {
            if (conditionDelegate == null)
                return from customer in DataSource.customers select customer;
            else
            {
                List<Customer> listCustomerByCondition = DataSource.customers.FindAll(customer => conditionDelegate(customer));
                return listCustomerByCondition;
            }
        }
        #endregion
    }
}

