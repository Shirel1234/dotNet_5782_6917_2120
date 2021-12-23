using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class BaseStationForList
    {
        public int Id { set; get; }
        public int Name { set; get; }
        public int ChargeSlotsFree { set; get; }
        public int ChargeSlotsCatch { set; get; }
        public override string ToString()
        {
            return String.Format($"Id: {Id}, Name:{Name}, Free Charge Slots: {ChargeSlotsFree}, Catch Charge Slots: {ChargeSlotsCatch}");
        }
    }
}
