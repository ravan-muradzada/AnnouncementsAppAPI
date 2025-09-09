using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CustomExceptions
{
    public class FailedToSendEmailException : Exception
    {
        public FailedToSendEmailException() : base("Email cannot be sent!") { }
        public FailedToSendEmailException(string message) : base(message) { }
        public FailedToSendEmailException(string message, Exception innerException): base(message, innerException) { }  
    }
}
