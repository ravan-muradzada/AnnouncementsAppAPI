using AnnouncemenetsAppAPI.Extensions;
using Application.DTOs.Announcement.Request;
using Application.DTOs.Announcement.Response;
using Application.DTOs.UserProfile.Request;
using Application.DTOs.UserProfile.Response;
using Application.InternalServiceInterfaces.IUserProfileServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnnouncemenetsAppAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class UserProfileController : ControllerBase
    {
        #region Fields
        private readonly IUserInfo_UserProfileService _userInfo_UserProfileService;
        private readonly ITwoFactorAuth_UserProfileService _twoFactorAuth_UserProfileService;
        private readonly IAnnouncement_UserProfileService _announcement_UserProfileService;
        #endregion

        #region Constructor
        public UserProfileController(IUserInfo_UserProfileService userInfo_UserProfileService, ITwoFactorAuth_UserProfileService twoFactorAuth_UserProfileService, IAnnouncement_UserProfileService announcement_UserProfileService)
        {
            _userInfo_UserProfileService = userInfo_UserProfileService;
            _twoFactorAuth_UserProfileService = twoFactorAuth_UserProfileService;
            _announcement_UserProfileService = announcement_UserProfileService;
        }
        #endregion

        #region GetUserProfile
        [HttpGet]
        public async Task<IActionResult> GetUserProfile()
        {
            Guid userId = User.GetUserId();
            UserProfileResponse response = await _userInfo_UserProfileService.GetUser(userId);
            return Ok(response);
        }
        #endregion

        #region ChangeEmail
        [HttpPost]
        public async Task<IActionResult> ChangeEmail(ChangeEmailRequest request)
        {
            Guid userId = User.GetUserId();
            await _userInfo_UserProfileService.ChangeEmail(userId, request);
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
            UserProfileResponse response = await _userInfo_UserProfileService.VerifyEmailChange(userId, request);
            return Ok(response);
        }
        #endregion

        #region ChangeUsername
        [HttpPut]
        public async Task<IActionResult> ChangeUsername(ChangeUsernameRequest request)
        {
            Guid userId = User.GetUserId();
            UserProfileResponse response = await _userInfo_UserProfileService.ChangeUsername(userId, request);
            return Ok(response);
        }
        #endregion

        #region ChangePassword
        [HttpPut]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            Guid userId = User.GetUserId();
            await _userInfo_UserProfileService.ChangePassword(userId, request);
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
            await _twoFactorAuth_UserProfileService.EnableTwoFactorAuth(userId);

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
            await _twoFactorAuth_UserProfileService.DisableTwoFactorAuth(userId);

            return Ok(new
            {
                Message = "Two factor auth has been disabled!"
            });
        }
        #endregion

        #region GetUsersAllAnnouncements
        [HttpGet]
        public async Task<IActionResult> GetUsersAllAnnouncements([FromQuery] bool isPublished, CancellationToken ct = default)
        {
            Guid userId = User.GetUserId();
            var response = await _announcement_UserProfileService.GetUsersAllAnnouncements(userId, isPublished, ct);
            return Ok(response);
        }
        #endregion

        #region GetUsersPagedAnnouncements
        [HttpGet]
        public async Task<IActionResult> GetUsersPagedAnnouncements(
            [FromQuery] int page,
            [FromQuery] int pageSize,
            [FromQuery] bool isPublished,
            [FromQuery] string? search = null,
            [FromQuery] string? category = null,
            [FromQuery] bool? isPinned = null,
            CancellationToken ct = default)
        {
            Guid userId = User.GetUserId();
            var response = await _announcement_UserProfileService.GetUsersPagedAnnouncements(userId, page, pageSize, isPublished, search, category, isPinned, ct);
            return Ok(response);
        }
        #endregion

        #region CreateAnnouncement
        [HttpPost]
        public async Task<IActionResult> CreateAnnouncement(CreateAnnouncementRequest request, CancellationToken ct)
        {
            Guid userId = User.GetUserId();
            AnnouncementResponse response = await _announcement_UserProfileService.CreateAnnouncement(userId, request, ct);
            return Ok(response);
        }
        #endregion

        #region DeleteAnnouncement
        [HttpDelete("{announcementId:guid}")]
        public async Task<IActionResult> DeleteAnnouncement(Guid announcementId, CancellationToken ct)
        {
            Guid userId = User.GetUserId();
            await _announcement_UserProfileService.DeleteAnnouncement(userId, announcementId, ct);
            return NoContent();
        }
        #endregion

        #region UpdateAnnouncement
        [HttpPut("{announcementId:guid}")]
        public async Task<IActionResult> UpdateAnnouncement(Guid announcementId, UpdateAnnouncementRequest request, CancellationToken ct)
        {
            Guid userId = User.GetUserId();
            AnnouncementResponse response = await _announcement_UserProfileService.UpdateAnnouncement(userId, announcementId, request, ct);
            return Ok(response);
        }
        #endregion
    }
}
