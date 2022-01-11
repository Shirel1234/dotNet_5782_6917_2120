using BO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
   internal class Simulator
    {
        private const double VELOCITY = 1.0;
        private const int DELAY = 500;
        private const double TIME_STEP = DELAY/1000;
        private const double STEP = VELOCITY / TIME_STEP;
       public Simulator(int id, Action updateDelegate, Func<bool> stopDelegate, BL bl)
        {
            Drone myDrone = bl.GetDrone(id);
            while (stopDelegate()==false)
            {
                while(bl.GetAllParcelsWithoutDrone().Count()!=0)
                {
                    while (myDrone.Battery >= 0 && myDrone.DroneStatus==DroneStatuses.free)
                    {
                        bl.UpdateParcelToDrone(id);
                        updateDelegate();
                        bl.UpdateParcelPickedUpByDrone(id);
                        System.Threading.Thread.Sleep(500);
                        updateDelegate();
                        bl.UpdateDeliveredParcelByDrone(id);
                        System.Threading.Thread.Sleep(500);
                        updateDelegate();
                    }
                    bl.UpdateSendingDroneToCharge(id);
                    updateDelegate();
                    bl.UpdateReleasingDroneFromCharge(id);
                    updateDelegate();


                }

            }
        }


    }
}
