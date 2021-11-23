using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
   public class CustomerList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Phone { get; set; }
        public int CountParcelsSend { get; set; }
        public int CountParcelsNotProvide { get; set; }
        public int CountParcelsGet { get; set; }
        public int CountParcelsInWay { get; set; }
        public override string ToString()
        {
            return String.Format($"({Id}, /n{Name}, /n{Phone}, /n{CountParcelsSend})/n{CountParcelsNotProvide}/n{CountParcelsInWay}");
        }
    }
}
