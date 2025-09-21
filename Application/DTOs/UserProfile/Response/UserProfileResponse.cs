using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.UserProfile.Response
{
    public sealed record UserProfileResponse(Guid Id, string UserName, string Email);
}
