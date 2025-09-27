using AnnouncemenetsAppAPI.Extensions;
using Application.InternalServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnnouncemenetsAppAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class JoinController : ControllerBase
    {
        #region Fields
        private readonly IJoinService _joinService;
        #endregion

        #region Constructor
        public JoinController(IJoinService joinService)
        {
            _joinService = joinService;
        }
        #endregion

        #region JoinAnnouncement
        [HttpPost("{announcementId:guid}")]
        public async Task<IActionResult> JoinAnnouncement(Guid announcementId, CancellationToken ct = default)
        {
            Guid userId = User.GetUserId();
            await _joinService.JoinAnnouncementAsync(announcementId, userId, ct);
            return Ok(new
            {
                Message = "Successfully Joined!"
            });
        }
        #endregion

        #region LeaveAnnouncement
        [HttpPost("{announcementId:guid}")]
        public async Task<IActionResult> LeaveAnnouncement(Guid announcementId, CancellationToken ct = default)
        {
            Guid userId = User.GetUserId();
            await _joinService.LeaveAnnouncementAsync(announcementId, userId, ct);
            return Ok(new
            {
                Message = "Successfully Disjoined!"
            });
        }
        #endregion
    }
}
