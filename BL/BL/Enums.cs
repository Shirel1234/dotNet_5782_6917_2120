using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public enum WeightCategories { easy, medium, heavy };
        public enum DroneStatuses { free, maintenace, sending };
        public enum Priorities { normal, express, emergency };
        public enum ParcelStatuses { requested, scheduled, pickedUp, delivered };
    }
}
