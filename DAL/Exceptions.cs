using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public class Exceptions
        {
            public class AddingExceptions:Exception
            {
                public AddingExceptions() : base() { }//חייב כזה בנאי?
                public AddingExceptions(int id,string message) : base(message) { }//האם זה אומר שיודפס קודם המספר שהתקבל ואז ההודעה?
                public AddingExceptions(string message) : base(message) { }
                override public string ToString()//חייב לדרוס אותה? האם ככה מדפיסים את החריגה?
                {
                    return Message + "\n";
                }


            }
            public class UpdateExceptions:Exception
            {
                public UpdateExceptions() : base() { }
                public UpdateExceptions(int id, string message) : base(message) { }
                override public string ToString()
                {
                    return Message + "\n";
                }
            }
            public class ViewExceptions : Exception
            {
                public ViewExceptions() : base() { }
                public ViewExceptions(int id, string message) : base(message) { }
                override public string ToString()
                {
                    return Message + "\n";
                }
            }
        }
    } 
}
