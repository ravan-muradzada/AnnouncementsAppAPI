﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Auth.Request
{
    public class SendOtpRequest
    {
        [Required(ErrorMessage = "Email is required to send otp to email!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = default!;
    }
}
