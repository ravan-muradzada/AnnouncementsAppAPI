﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CustomExceptions
{
    public class IdentityException : Exception
    {
        public IdentityException() : base("Identity operation failed!") { }
        public IdentityException(string message) : base(message) { }
        public IdentityException(string message, Exception innerException) : base(message, innerException) { }
    }
}
