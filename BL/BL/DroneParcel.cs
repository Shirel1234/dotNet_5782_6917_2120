using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
   public class DroneParcel
    {
      public int Id { set; get; }
      public double Battery { set; get; }
      public Location LocationNow { set; get; }
        public override string ToString()
        {
            return String.Format($"{Id}, {Battery}, {LocationNow}, ");
        }
    }
}
//לקוח בחבילה-כן-חבילה,חבילה בהעברה
//חבילה אצל לקוח-כן-לקוח
//חבילה בהעברה-כן-רחפן
//רחפן בחבילה-כן-חבילה
//רחפן בטעינה-כן-תחנת בסיס