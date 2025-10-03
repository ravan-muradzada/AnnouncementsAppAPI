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

        #region GetAllAnnouncements
        public async Task<List<AnnouncementResponse>> GetAllAnnouncements(CancellationToken ct = default)
        {
            List<Announcement> announcements = await _announcementRepository.GetAllAsync(null, true, false, ct);

            return _mapper.Map<List<AnnouncementResponse>>(announcements);
        }
        #endregion

        #region GetAnnouncement
        public async Task<AnnouncementResponse> GetAnnouncement(Guid announcemenetId, CancellationToken ct = default)
        {
            Announcement? announcement = await _announcementRepository.GetByIdAsync(announcemenetId, null, true, false, ct) ?? throw new ObjectNotFoundException("Announcement Not Found!");

            return _mapper.Map<AnnouncementResponse>(announcement);
        }
        #endregion

        #region GetPagedAnnouncements
        public async Task<PagedResult<AnnouncementResponse>> GetPagedAnnouncements(int page, int pageSize, string? search = null, string? category = null, bool? isPinned = null, CancellationToken ct = default)
        {
            var pagedResult = await _announcementRepository.GetPagedAsync(page, pageSize, null, true, search, category, isPinned, false, ct);
            return new PagedResult<AnnouncementResponse>
            {
                Items = _mapper.Map<List<AnnouncementResponse>>(pagedResult.Items),
                TotalCount = pagedResult.TotalCount,
                PageSize = pagedResult.PageSize,
                CurrentPageNumber = pagedResult.CurrentPageNumber,
            };
        }
        #endregion
    }
}
