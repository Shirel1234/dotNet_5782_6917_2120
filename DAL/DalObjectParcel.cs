using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalObject
{
    public partial class DalObject
    {
        /// <summary>
        /// the function check if this station is exist yet
        /// </summary>
        /// <param name="id"></param>
        /// <returns> true if it is exist and else false</returns>
        public bool CheckParcel(int id)
        {
            return DataSource.parcels.Any(parcel => parcel.CodeParcel == id);
        }
        /// <summary> 
        /// the function gets an object of parcel and adds it to the list of parcels
        /// </summary>
        /// <param name="p"> a parcel</param>
        public void AddParcel(Parcel p)
        {
            if (CheckParcel(p.CodeParcel))
                throw new AlreadyExistException("The bus already exist in the system");
            DataSource.parcels.Add(p);
        }
        /// <summary>
        /// the function searchs the parcel with the id that it got
        /// </summary>
        /// <param name="id"></param>
        /// <returns>the details of this parcel</returns>
        public Parcel GetParcel(int id)
        {
            Parcel p = DataSource.parcels.Find(parcel => parcel.CodeParcel == id);
            return p;
        }
        /// <summary>
        /// the function show the list of parcels
        /// </summary>
        /// <returns>list of parcels</returns>
        public IEnumerable<Parcel> GetParcels()
        {
            return from parcel in DataSource.parcels select parcel;
        }

    }
}