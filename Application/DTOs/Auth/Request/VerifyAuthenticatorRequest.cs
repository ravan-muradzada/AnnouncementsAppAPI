using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Auth.Request
{
    public class VerifyAuthenticatorRequest
    {
        [Required(ErrorMessage = "You need to attach the code to authenticate!")]
        public string Code { get; set; } = string.Empty;
    }
}
