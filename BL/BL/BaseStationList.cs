using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class BaseStationList
    {
        public int Id { set; get; }
        public int Name { set; get; }
        public int ChargeSlotsFree { set; get; }
        public int ChargeSlotsCatch { set; get; }
    }
}
