using Application.DTOs.Announcement.Response;
using Application.ExternalServiceInterfaces;
using Application.InternalServiceInterfaces;
using AutoMapper;
using Domain.Common;
using Domain.CustomExceptions;
using Domain.Entities;
using Domain.RepositoryInterfaces;

namespace Application.InternalServices
{
    public class AdminService : IAdminService
    {
        #region Fields
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public AdminService(IAnnouncementRepository announcementRepository, IEmailService emailService, IMapper mapper)
        {
            _announcementRepository = announcementRepository;
            _emailService = emailService;
            _mapper = mapper;
        }
        #endregion

        #region DeleteAnnouncement
        public async Task DeleteAnnouncement(Guid announcementId, CancellationToken ct = default)
        {
            Announcement? announcement = await _announcementRepository.GetByIdAsync(announcementId, null, false, ct) ?? throw new ObjectNotFoundException("Announcement not found");

            await _announcementRepository.DeleteAsync(announcement, ct);

            await _emailService.SendEmail(
                announcement.Author.Email!,
                "Announcement Deleted",
                $"Your announcement titled '{announcement.Title}' has been deleted by the admin.");
        }
        #endregion

        #region PublishAnnouncement
        public async Task PublishAnnouncement(Guid announcementId, CancellationToken ct = default)
        {
            Announcement? announcement = await _announcementRepository.GetByIdAsync(announcementId, null, false, ct) ?? throw new ObjectNotFoundException("Announcement not found");

            announcement.IsPublished = true;
            announcement.PublishedAt = DateTime.UtcNow;
            await _announcementRepository.SaveChangesAsync(announcement, ct);

            await _emailService.SendEmail(
                announcement.Author.Email!,
                "Announcement Published",
                $"Your announcement titled '{announcement.Title}' has been published.");
        }
        #endregion

        #region GetAllAnnouncements
        public async Task<List<AnnouncementResponse>> GetAllAnnouncements(CancellationToken ct = default)
        {
            List<Announcement> announcements = await _announcementRepository.GetAllAsync(null, false, ct);
            return _mapper.Map<List<AnnouncementResponse>>(announcements);
        }
        #endregion

        #region GetAnnouncementById
        public async Task<AnnouncementResponse> GetAnnouncementById(Guid announcementId, CancellationToken ct = default)
        {
            Announcement? announcement = await _announcementRepository.GetByIdAsync(announcementId, null, false, ct) ?? throw new ObjectNotFoundException("Announcement not found");
            return _mapper.Map<AnnouncementResponse>(announcement);
        }
        #endregion

        #region GetPagedAnnouncements
        public async Task<PagedResult<AnnouncementResponse>> GetAnnouncements(int pageNumber, int pageSize, Guid? userId = null, string? search = null, string? category = null, bool? isPinned = null, CancellationToken ct = default)
        {
            var announcements = await _announcementRepository.GetPagedAsync(pageNumber, pageSize, userId, false, search, category, isPinned, ct);
            return _mapper.Map<PagedResult<AnnouncementResponse>>(announcements);
        }
        #endregion
    }
}
