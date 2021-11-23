using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace BL
{
   public partial class BLObject: IBL//.BO//לא צריך לירוש כאן מאיי בי אל? 
    {
        #region
        IDAL.DO.IDal dl;
        public BLObject()
        {
            dl = new DalObject.DalObject();
        }
       
        #endregion
    }
}
