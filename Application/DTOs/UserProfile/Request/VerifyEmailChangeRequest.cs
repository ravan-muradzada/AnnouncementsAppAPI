using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.UserProfile.Request
{
    public sealed record VerifyEmailChangeRequest(string OTP);
}
