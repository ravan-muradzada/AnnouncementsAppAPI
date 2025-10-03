using Application.InternalServiceInterfaces.IUserProfileServices;
using Domain.CustomExceptions;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

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
        public async Task EnableTwoFactorAuth(Guid userId, CancellationToken ct = default)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(userId.ToString()) ?? throw new ObjectNotFoundException("User not found!");
            await _userManager.SetTwoFactorEnabledAsync(user, true);
        }
        #endregion

        #region DisableTwoFactorAuth
        public async Task DisableTwoFactorAuth(Guid userId, CancellationToken ct = default)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(userId.ToString()) ?? throw new ObjectNotFoundException("User not found!");
            await _userManager.SetTwoFactorEnabledAsync(user, false);
        }
        #endregion
    }
}
