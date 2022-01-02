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
        public void UpDateParcel(Parcel p)
        {
            Parcel myParcel = DataSource.parcels.Find(x => x.CodeParcel == p.CodeParcel);
            if (myParcel.CodeParcel != p.CodeParcel)
                throw new DoesntExistException("This parcel doesn't exist in the system");
            myParcel.CodeParcel = p.CodeParcel;//נראה לי מיותר לעדכן, כי אף פעם לא משנים את מספר הזיהוי וגם בודקים באמצעותו קודם
            myParcel.SenderId = p.SenderId;
            myParcel.TargetId = p.TargetId;
            myParcel.Weight = p.Weight;
            myParcel.Priority = p.Priority;
            myParcel.DroneId = p.DroneId;
            myParcel.Requested = p.Requested;
            myParcel.Scheduled = p.Scheduled;
            myParcel.PickedUp = p.PickedUp;
            myParcel.Delivered = p.Delivered;
            DataSource.parcels.Remove(p);
            DataSource.parcels.Add(myParcel);
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
        public IEnumerable<Parcel> GetParcelsByCondition(Func<Parcel, bool> conditionDelegate = null)
        {
            if (conditionDelegate == null)
                return from parcel in DataSource.parcels select parcel;
            else
            {
                List<Parcel> listParcelByCondition = DataSource.parcels.FindAll(parcel => conditionDelegate(parcel));
                return listParcelByCondition;
            }
        }
        public void RemoveParcel(int id)
        {
            Parcel p = DataSource.parcels.Find(parcel => parcel.CodeParcel == id);
            if(p.CodeParcel==0)
                throw new DoesntExistException ("This parcel doesn't exist in the system");
            DataSource.parcels.Remove(p);
        }
    }
}
