using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.UserProfile.Request
{
    public class ChangeEmailRequest
    {
        [Required(ErrorMessage = "You need to provide a new email address.")]
        [EmailAddress(ErrorMessage = "The email address provided is not valid.")]
        public string NewEmail { get; set; } = default!;
    }
}
