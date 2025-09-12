using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.UserProfile.Request
{
    public class VerifyEmailChangeRequest
    {
        [Required(ErrorMessage = "OTP is required")]
        public string OTP { get; set; } = default!;
    }
}
