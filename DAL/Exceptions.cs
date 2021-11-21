using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace IDAL
{
    namespace DO
    {
        public class DoesntExistException : Exception
        {
            public DoesntExistException() : base() { }
            public DoesntExistException(string message) : base(message) { }
            public DoesntExistException(string message, Exception inner) : base(message, inner) { }
            protected DoesntExistException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        }

        public class AlreadyExistException : Exception
        {
            public AlreadyExistException() : base() { }
            public AlreadyExistException(string message) : base(message) { }
            public AlreadyExistException(string message, Exception inner) : base(message, inner) { }
            protected AlreadyExistException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        }

        public class InvalidInputException : Exception
        {
            public InvalidInputException() : base() { }
            public InvalidInputException(string message) : base(message) { }
            public InvalidInputException(string message, Exception inner) : base(message, inner) { }
            protected InvalidInputException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        }
    
    }
}

