using Application.DTOs.UserProfile.Request;
using Application.DTOs.UserProfile.Response;
using Application.InternalServiceInterfaces;
using Application.InternalServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using starter_project_template.Extensions;

namespace starter_project_template.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class UserProfileController : ControllerBase
    {
        #region Fields
        private readonly IUserProfileService _userProfileService;
        #endregion

        #region Constructor
        public UserProfileController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }
        #endregion

        #region GetUserProfile
        [HttpGet]
        public async Task<IActionResult> GetUserProfile()
        {
            Guid userId = User.GetUserId();
            UserProfileResponse response = await _userProfileService.GetUser(userId);
            return Ok(response);
        }
        #endregion

        #region ChangeEmail
        [HttpPost]
        public async Task<IActionResult> ChangeEmail(ChangeEmailRequest request)
        {
            Guid userId = User.GetUserId();
            await _userProfileService.ChangeEmail(userId, request);
            return Ok(new
            {
                Message = "Verification code has been sent to new email address. Please verify to complete email change."
            });
        }
        #endregion

        #region VerifyEmailChange
        [HttpPut]
        public async Task<IActionResult> VerifyEmailChange(VerifyEmailChangeRequest request)
        {
            Guid userId = User.GetUserId();
            UserProfileResponse response = await _userProfileService.VerifyEmailChange(userId, request);
            return Ok(response);
        }
        #endregion

        #region ChangeUsername
        [HttpPut]
        public async Task<IActionResult> ChangeUsername(ChangeUsernameRequest request)
        {
            Guid userId = User.GetUserId();
            UserProfileResponse response = await _userProfileService.ChangeUsername(userId, request);
            return Ok(response);
        }
        #endregion

        #region ChangePassword
        [HttpPut]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            Guid userId = User.GetUserId();
            await _userProfileService.ChangePassword(userId, request);
            return Ok(new
            {
                Message = "Password has been changed successfully!"
            });
        }
        #endregion

        #region EnableTwoFactorAuth
        [HttpPost]
        public async Task<IActionResult> EnableTwoFactorAuth()
        {
            Guid userId = User.GetUserId();
            await _userProfileService.EnableTwoFactorAuth(userId);

            return Ok(new
            {
                Message = "Two factor auth has been enabled!"
            });
        }
        #endregion

        #region DisableTwoFactorAuth
        [HttpPost]
        public async Task<IActionResult> DisableTwoFactorAuth()
        {
            Guid userId = User.GetUserId();
            await _userProfileService.DisableTwoFactorAuth(userId);

            return Ok(new
            {
                Message = "Two factor auth has been disabled!"
            });
        }
        #endregion
    }
}
