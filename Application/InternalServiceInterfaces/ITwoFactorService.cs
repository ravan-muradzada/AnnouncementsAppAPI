using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.InternalServiceInterfaces
{
    public interface ITwoFactorService
    {
        Task SendTwoFactorCode(ApplicationUser user);
        Task<bool> VerifyTwoFactorCode(ApplicationUser user, string code);
    }
}
