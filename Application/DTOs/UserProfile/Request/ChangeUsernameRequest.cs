using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.UserProfile.Request
{
    public class ChangeUsernameRequest
    {
        [Required(ErrorMessage = "Username is required")]
        public string NewUsername { get; set; } = default!;
    }
}
