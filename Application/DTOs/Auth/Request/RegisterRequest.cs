using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Auth.Request
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Username is required to register!")]
        public string UserName { get; set; } = default!;

        [Required(ErrorMessage = "Email is required to register!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = default!;
        [Required(ErrorMessage = "Password is required to register!")]
        public string Password { get; set; } = default!;
    }
}
