using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    public partial class BLObject
    {
        public void AddBaseStation(BaseStation b) 
        {
                if (b.Id < 0)
                    throw new AddingProblemException("The ID number must be a positive number");
                if (b.ChargeSlots < 0)
                    throw new AddingProblemException("The number of the free charging positions must be a positive number");
                if (b.Location.Longitude < -180 || b.Location.Longitude > 180)
                    throw new AddingProblemException("The longitude must be between -180 to 180");
                if (b.Location.Latitude < -90 || b.Location.Latitude > 90)
                    throw new AddingProblemException("The latitude must be between -90 to 90");
            try
            {
                IDAL.DO.BaseStation doStation = new IDAL.DO.BaseStation()
                {
                    CodeStation = b.Id,
                    NameStation = b.Name,
                    ChargeSlots = b.ChargeSlots,
                    Longitude = b.Location.Longitude,
                    Latitude = b.Location.Latitude
                };
                dl.AddStation(doStation);
            }

            catch (Exception ex)
            {
              throw new AddingProblemException("Can't add this baseStation", ex);
            }
        }
        public void UpdateBaseStation(int id, string model)
        {
            I
        }
    }
}
