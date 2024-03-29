﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DO
{
    public struct Parcel
    {
        public int CodeParcel { get; set; }
        public int SenderId { get; set; }
        public int TargetId { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public int DroneId { get; set; }
        public DateTime? Requested { get; set; }
        public DateTime? Scheduled { get; set; }
        public DateTime? PickedUp { get; set; }
        public DateTime? Delivered { get; set; }
        public override string ToString()
        {
            return String.Format($"(Code Parcel: {CodeParcel}, SenderId: {SenderId},TargetId: {TargetId}, Weight: {Weight}, Priority: {Priority}, DroneId: {DroneId}, Requested: {Requested}, Scheduled: {Scheduled}, Picked Up: {PickedUp}, Delivered: {Delivered})");
        }




    }
}


