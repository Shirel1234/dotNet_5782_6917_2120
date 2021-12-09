using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class ParcelInWay
    {
        public int Id { set; get; }
        public bool IsInWay { set; get; }
        public Priorities Priority { set; get; }
        public WeightCategories Weight { set; get; }
        public CustomerParcel Sender { set; get; }
        public CustomerParcel Target { set; get; }
        public Location LocationPickedUp { set; get; }
        public Location LocationTarget { set; get; }
        public double TransportDistance { set; get; }
        public override string ToString()
        {
            return String.Format($"{Id}, {IsInWay}, {Priority}, {Weight}, \n{Sender}, \n{Target}, \n{LocationPickedUp}, \n{LocationTarget}, \n{TransportDistance}");
        }
    }
}
