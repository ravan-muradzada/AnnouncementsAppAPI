using Application.ExternalServiceInterfaces;
using Application.InternalServiceInterfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.InternalServices
{
    public class TwoFactorService : ITwoFactorService
    {
        #region fields
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        #endregion

        #region constructor
        public TwoFactorService(UserManager<ApplicationUser> userManager, IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }
        #endregion

        #region SendTwoFactorCode
        public async Task SendTwoFactorCode(ApplicationUser user, CancellationToken ct = default)
        {
            var token = await _userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider);
            await _emailService.SendEmail(user.Email!, "Two factor authentication code", token);
        }
        #endregion

        #region VerifyTwoFactorCode
        public async Task<bool> VerifyTwoFactorCode(ApplicationUser user, string code, CancellationToken ct = default)
        {
            return await _userManager.VerifyTwoFactorTokenAsync(
            user, TokenOptions.DefaultEmailProvider, code);
        }
        #endregion
    }
}
