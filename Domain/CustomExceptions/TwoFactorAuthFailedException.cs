using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CustomExceptions
{
    public class TwoFactorAuthFailedException : Exception
    {
        public TwoFactorAuthFailedException() : base("Two factor authentication has failed!") { }
        public TwoFactorAuthFailedException(string message) : base(message) { }
        public TwoFactorAuthFailedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
