using Application.DTOs.Announcement.Request;
using Application.DTOs.Announcement.Response;
using Application.InternalServiceInterfaces;
using AutoMapper;
using Domain.Common;
using Domain.CustomExceptions;
using Domain.Entities;
using Domain.RepositoryInterfaces;

namespace Application.InternalServices
{
    public class AnnouncementService : IAnnouncementService
    {
        #region Fields
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public AnnouncementService(IAnnouncementRepository announcementRepository, IMapper mapper)
        {
            _announcementRepository = announcementRepository;
            _mapper = mapper;
        }
        #endregion

        #region CreateAnnouncement
        public async Task<AnnouncementResponse> CreateAnnouncement(Guid userId, CreateAnnouncementRequest request, CancellationToken ct = default)
        {
            Announcement announcement = _mapper.Map<Announcement>(request);
            Announcement response = await _announcementRepository.AddAync(announcement, ct);

            return _mapper.Map<AnnouncementResponse>(response);
        }
        #endregion

        #region DeleteAnnouncement
        public async Task DeleteAnnouncement(Guid userId, Guid announcementId, CancellationToken ct)
        {
            Announcement? announcement = await _announcementRepository.GetByIdAsync(announcementId) ?? throw new ObjectNotFoundException("Announcemenet Not Found!");

            await _announcementRepository.DeleteAsync(announcement);
        }
        #endregion

        #region GetAllAnnouncements
        public async Task<List<AnnouncementResponse>> GetAllAnnouncements(CancellationToken ct)
        {
            List<Announcement> announcements = await _announcementRepository.GetAllAsync();
            return _mapper.Map<List<AnnouncementResponse>>(announcements);
        }
        #endregion

        #region GetAnnouncement
        public async Task<AnnouncementResponse> GetAnnouncement(Guid announcemenetId, CancellationToken ct = default)
        {
            Announcement? announcement = await _announcementRepository.GetByIdAsync(announcemenetId, ct) ?? throw new ObjectNotFoundException("Announcemenet Not Found!");
            return _mapper.Map<AnnouncementResponse>(announcement);
        }
        #endregion

        #region GetPagedAnnouncements
        public async Task<PagedResult<AnnouncementResponse>> GetPagedAnnouncements(int page, int pageSize, string? search = null, string? category = null, bool? isPublished = null, bool? isPinned = null, CancellationToken ct = default)
        {
            var pagedResult = await _announcementRepository.GetPagedAsync(page, pageSize, search, category, isPublished, isPinned, ct);
            return _mapper.Map<PagedResult<AnnouncementResponse>>(pagedResult);
        }
        #endregion

        #region UpdateAnnouncement
        public async Task<AnnouncementResponse> UpdateAnnouncement(Guid userId, Guid announcementId, UpdateAnnouncementRequest request, CancellationToken ct = default)
        {
            Announcement? announcement = await _announcementRepository.GetByIdAsync(announcementId, ct) ?? throw new ObjectNotFoundException("Announcement Not Found!");

            if (request.Equals(null))
            {
                return _mapper.Map<AnnouncementResponse>(announcement);
            }

            if (request.Title is not null && announcement.Title != request.Title)
            {
                announcement.Title = request.Title;
            }

            if (request.Content is not null && announcement.Content != request.Content)
            {
                announcement.Content = request.Content;
            }

            if (request.Category is not null && announcement.Category != request.Category)
            {
                announcement.Category = request.Category;
            }

            if (request.ExpiresAt is not null && announcement.ExpiresAt != request.ExpiresAt)
            {
                announcement.ExpiresAt = request.ExpiresAt;
            }

            await _announcementRepository.SaveChangesAsync(announcement, ct);
            return _mapper.Map<AnnouncementResponse>(announcement);
        }
        #endregion
    }
}
