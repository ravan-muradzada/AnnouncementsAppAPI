using AnnouncemenetsAppAPI.Extensions;
using Application.DTOs.Announcement.Request;
using Application.DTOs.Announcement.Response;
using Application.InternalServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnnouncemenetsAppAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class AnnouncementController : ControllerBase
    {
        #region Fields
        private readonly IAnnouncementService _announcementService;
        #endregion

        #region Constructor
        public AnnouncementController(IAnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }
        #endregion

        #region CreateAnnouncement
        [HttpPost]
        public async Task<IActionResult> CreateAnnouncement(CreateAnnouncementRequest request, CancellationToken ct)
        {
            Guid userId = User.GetUserId();
            AnnouncementResponse response = await _announcementService.CreateAnnouncement(userId, request, ct);
            return Ok(response);
        }
        #endregion

        #region DeleteAnnouncement
        [HttpDelete("{announcementId:guid}")]
        public async Task<IActionResult> DeleteAnnouncement(Guid announcementId, CancellationToken ct)
        {
            Guid userId = User.GetUserId();
            await _announcementService.DeleteAnnouncement(userId, announcementId, ct);
            return NoContent();
        }
        #endregion

        #region UpdateAnnouncement
        [HttpPut("{announcementId:guid}")]
        public async Task<IActionResult> UpdateAnnouncement(Guid announcementId, UpdateAnnouncementRequest request, CancellationToken ct)
        {
            Guid userId = User.GetUserId();
            AnnouncementResponse response = await _announcementService.UpdateAnnouncement(userId, announcementId, request, ct);
            return Ok(response);
        }
        #endregion

        #region GetAnnouncement
        [HttpGet("{announcementId:guid}")]
        public async Task<IActionResult> GetAnnouncement(Guid announcementId, CancellationToken ct)
        {
            AnnouncementResponse response = await _announcementService.GetAnnouncement(announcementId, ct);
            return Ok(response);
        }
        #endregion

        #region GetAllAnnouncements
        [HttpGet]
        public async Task<IActionResult> GetAllAnnouncements(CancellationToken ct)
        {
            List<AnnouncementResponse> response = await _announcementService.GetAllAnnouncements(ct);
            return Ok(response);
        }
        #endregion

        #region GetPagedAnnouncements
        [HttpGet]
        public async Task<IActionResult> GetPagedAnnouncements(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] string? category = null,
            [FromQuery] bool? isPinned = null,
            CancellationToken ct = default)
        {
            var response = await _announcementService.GetPagedAnnouncements(page, pageSize, search, category, isPinned, ct);
            return Ok(response);
        }
        #endregion
    }
}
