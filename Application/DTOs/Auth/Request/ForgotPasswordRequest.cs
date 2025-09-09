using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Auth.Request
{
    public class ForgotPasswordRequest
    {
        [Required(ErrorMessage = "To reset password, you need to enter your email!")]
        public string Email { get; set; } = default!;
    }
}
