using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CustomExceptions
{
    public class NotConfirmedException : Exception
    {
        public NotConfirmedException() : base("Subject not confirmed!") { }
        public NotConfirmedException(string message) : base(message) { }
        public NotConfirmedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
