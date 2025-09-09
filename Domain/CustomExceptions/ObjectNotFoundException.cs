using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CustomExceptions
{
    public class ObjectNotFoundException : Exception
    {
        public ObjectNotFoundException() : base("Object Not Found!") { }
        public ObjectNotFoundException(string message) : base(message) { }
        public ObjectNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
