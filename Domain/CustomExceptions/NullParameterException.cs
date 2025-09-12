using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CustomExceptions
{
    public class NullParameterException : Exception
    {
        public NullParameterException() : base("Parameter(s) cannot be null!") { }
        public NullParameterException(string message) : base(message) { }
        public NullParameterException(string message, Exception innerException) : base(message, innerException) { }
    }
}
