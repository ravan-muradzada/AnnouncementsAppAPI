using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Auth.Request
{
    public class RefreshTokenRequest
    {
        [Required(ErrorMessage = "You need to attach the previous refresh token to generate new one!")]
        public string RefreshToken { get; set; } = default!;
    }
}
