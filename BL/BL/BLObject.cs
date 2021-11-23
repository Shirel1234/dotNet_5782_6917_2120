using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL
{
   public partial class BLObject: IBl//.BO//לא צריך לירוש כאן מאיי בי אל? 
    {
        #region
        IDAL.DO.IDal dl;
        public BLObject()
        {
            dl = new DalObject.DalObject();
            double[] arr = dl.AskElectrical();
            double free = arr[0];
            double easy = arr[1];
            double medium = arr[2];
            double heavy = arr[3];
            double chargingRate = arr[4];
        }
       
        #endregion
    }
}
