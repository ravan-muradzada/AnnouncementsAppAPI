using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.InternalServiceInterfaces.IUserProfileServices
{
    public interface ITwoFactorAuth_UserProfileService
    {
        Task EnableTwoFactorAuth(Guid userId);
        Task DisableTwoFactorAuth(Guid userId);
    }
}
