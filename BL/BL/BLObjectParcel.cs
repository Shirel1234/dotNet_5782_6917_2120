﻿using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    public partial class BLObject
    {
            public void AddParcel(Parcel p)
            {
            if (p.SenderCustomerId < 100000000 || p.SenderCustomerId > 999999999)
                throw new AddingProblemException("The id of sender must contain 9 digits");
           if (p.TargetCustomerId < 100000000 || p.TargetCustomerId > 999999999)
                throw new AddingProblemException("The id of target must contain 9 digits");
            if (p.Weight < 0)///operator>
                throw new AddingProblemException("The weight isn't valid");
            if (p.Priority < 0)///operator>
                throw new AddingProblemException("The priority isn't valid");
            p.Requested = DateTime.Now;
            p.Scheduled = new DateTime(0);
            p.PickedUp = new DateTime(0);
            p.Delivered = new DateTime(0);
            p.DroneId = 0;
            try
            {
                    IDAL.DO.Parcel doParcel = new IDAL.DO.Parcel()
                    {
                        CodeParcel = p.CodeParcel,
                        SenderId= p.SenderCustomerId,
                        TargetId= p.TargetCustomerId,
                        Weight= (IDAL.DO.WeightCategories)p.Weight,
                        DroneId = p.DroneId,
                        Priority = (IDAL.DO.Priorities)p.Priority,
                        Delivered = p.Delivered,
                        Requested = p.Requested,
                        PickedUp=p.PickedUp,
                        Scheduled=p.Scheduled,
                    };
                    dl.AddParcel(doParcel);
                }

                catch (Exception ex)
                {
                    throw new AddingProblemException("Can't add this parcel", ex);
                }
            }

        }
}
