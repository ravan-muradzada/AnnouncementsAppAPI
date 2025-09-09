using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CustomExceptions
{
    public class InvalidAccessTokenException : Exception
    {
        public InvalidAccessTokenException() : base("Provided token is invalid!") { }
        public InvalidAccessTokenException(string message) : base(message) { }
        public InvalidAccessTokenException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
