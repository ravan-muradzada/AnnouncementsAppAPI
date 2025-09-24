using Application.ExternalServiceInterfaces;
using Application.InternalServiceInterfaces;
using Application.InternalServiceInterfaces.IUserProfileServices;
using AutoMapper;
using Domain.CustomExceptions;
using Domain.Entities;
using Domain.RepositoryInterfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.InternalServices.UserProfileServices
{
    public class TwoFactorAuth_UserProfileService : ITwoFactorAuth_UserProfileService
    {
        #region Fields
        private readonly UserManager<ApplicationUser> _userManager;
        #endregion

        #region Constructor
        public TwoFactorAuth_UserProfileService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        #endregion

        #region EnableTwoFactorAuth 
        public async Task EnableTwoFactorAuth(Guid userId)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null) throw new ObjectNotFoundException("User not found!");
            await _userManager.SetTwoFactorEnabledAsync(user, true);
        }
        #endregion

        #region DisableTwoFactorAuth
        public async Task DisableTwoFactorAuth(Guid userId)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null) throw new ObjectNotFoundException("User not found!");
            await _userManager.SetTwoFactorEnabledAsync(user, false);
        }
        #endregion
    }
}
