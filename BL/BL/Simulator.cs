﻿using BO;
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
        private const double VELOCITY = 1.0;//מהירות
        private const int DELAY = 500;//מחזור בשניות
        private const double TIME_STEP = DELAY/1000;
        private const double STEP = VELOCITY / TIME_STEP;
       public Simulator(int id, Action updateDelegate, Func<bool> stopDelegate, BL bl)
        {
            Stopwatch stopwatch = new Stopwatch();
            Drone myDrone;
            stopwatch.Start();
            lock (bl)
            {
                myDrone = bl.GetDrone(id);
            }
            while (stopDelegate()==false)
            {
                switch((DroneStatuses)myDrone.DroneStatus)
                {
                    case DroneStatuses.free:
                    {
                          if(!bl.UpdateParcelToDrone(myDrone.Id))
                          {
                                bl.UpdateSendingDroneToCharge(myDrone.Id);
                                myDrone = bl.GetDrone(myDrone.Id);
                          }

                            break;
                    }
                    case DroneStatuses.maintenace:
                    {
                            lock (bl)
                            {
                                if (myDrone.Battery == 100)
                                {
                                    bl.UpdateReleasingDroneFromCharge(myDrone.Id);
                                    myDrone = bl.GetDrone(myDrone.Id);
                                }

                                else
                                {
                                    myDrone.Battery += VELOCITY * bl.dal.AskElectrical()[4];
                                    System.Threading.Thread.Sleep(DELAY);
                                    if (myDrone.Battery > 100)
                                        myDrone.Battery = 100;
                                    bl.UpdateDrone(myDrone);
                                }
                            }
                            break;

                        }
                    case DroneStatuses.sending:
                    {
                            lock(bl)
                            {
                                Parcel p = bl.GetParcel(myDrone.ParcelInWay.Id);
                                if(!myDrone.ParcelInWay.IsInWay)
                                {
                                    if(myDrone.ParcelInWay.TransportDistance/ VELOCITY<=((TimeSpan)(DateTime.Now-p.Scheduled)).Seconds)
                                    {

                                    }
                                }
                                

                                //    updateDelegate();
                                //    bl.UpdateParcelPickedUpByDrone(myDrone.Id);
                                //    System.Threading.Thread.Sleep(500);
                                //    updateDelegate();
                                //    bl.UpdateDeliveredParcelByDrone(myDrone.Id);
                                //    System.Threading.Thread.Sleep(500);
                                //    updateDelegate();
                                //}
                                //bl.UpdateSendingDroneToCharge(myDrone.Id);
                                //updateDelegate();
                                //bl.UpdateReleasingDroneFromCharge(myDrone.Id);
                                //updateDelegate();


                            }
                            break;
                    }
                }

            }
       }

   }
}
