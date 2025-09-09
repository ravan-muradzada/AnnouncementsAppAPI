using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Auth.Request
{
    public class ResetPasswordRequest
    {
        [Required(ErrorMessage = "You need to enter your email to reset password.")]
        public string Email { get; set; } = default!;
        [Required(ErrorMessage = "You need to enter new password to reset password.")]
        public string NewPassword { get; set; } = default!;
    }
}
