using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Auth.Request
{
    public class VerifyOtpRequest
    {
        [Required(ErrorMessage = "Email should be sent to verify email!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "You need to send otp!")]
        public string Otp { get; set; } = default!;
    }
}
