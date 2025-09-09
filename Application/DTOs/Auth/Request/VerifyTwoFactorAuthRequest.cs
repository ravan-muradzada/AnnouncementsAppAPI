using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Auth.Request
{
    public class VerifyTwoFactorAuthRequest
    {
        [Required(ErrorMessage = "You need to enter your email to verify 2FA!")]
        public string Email { get; set; } = default!;
        [Required(ErrorMessage = "You need to enter the code to verify 2FA!")]
        public string Code { get; set; } = default!;
    }
}
