using Application.InternalServiceInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnnouncemenetsAppAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        #region Fields
        private readonly IAdminService _adminService;
        #endregion

        #region Constructor
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }
        #endregion

        #region GetAllAnnouncements
        [HttpGet]
        public async Task<IActionResult> GetAllAnnouncements(CancellationToken ct = default)
        {
            var announcements = await _adminService.GetAllAnnouncements(ct);
            return Ok(announcements);
        }
        #endregion

        #region GetAnnouncementById
        [HttpGet("{announcementId:guid}")]
        public async Task<IActionResult> GetAnnouncementById(Guid announcementId, CancellationToken ct = default)
        {
            var announcement = await _adminService.GetAnnouncementById(announcementId, ct);
            return Ok(announcement);
        }
        #endregion

        #region GetPagedAnnouncements
        public async Task<IActionResult> GetPagedAnnouncements(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] Guid? userId = null,
            [FromQuery] string? search = null,
            [FromQuery] string? category = null,
            [FromQuery] bool? isPinned = null,
            CancellationToken ct = default)
        {
            var pagedResult = await _adminService.GetAnnouncements(pageNumber, pageSize, userId, search, category, isPinned, ct);
            return Ok(pagedResult);
        }
        #endregion
    }
}
