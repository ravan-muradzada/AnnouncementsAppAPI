using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CustomExceptions
{
    public class NotAllowedException : Exception
    {
        public NotAllowedException() : base("Argument and process not allowed!") { }
        public NotAllowedException(string message) : base(message) { }
        public NotAllowedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
