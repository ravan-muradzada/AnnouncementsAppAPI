using Application.DTOs.Announcement.Request;
using Application.DTOs.Announcement.Response;
using Application.ExternalServiceInterfaces;
using Application.InternalServiceInterfaces;
using Application.InternalServiceInterfaces.IUserProfileServices;
using AutoMapper;
using Domain.Common;
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
    public class Announcement_UserProfileService : IAnnouncement_UserProfileService
    {
        #region Fields
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public Announcement_UserProfileService(IAnnouncementRepository announcementRepository, IMapper mapper)
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
        public async Task DeleteAnnouncement(Guid userId, Guid announcementId, CancellationToken ct = default)
        {
            Announcement? announcement = await _announcementRepository.GetByIdAsync(announcementId) ?? throw new ObjectNotFoundException("Announcemenet Not Found!");

            await _announcementRepository.DeleteAsync(announcement, ct);
        }
        #endregion

        #region UpdateAnnouncement
        public async Task<AnnouncementResponse> UpdateAnnouncement(Guid userId, Guid announcementId, UpdateAnnouncementRequest request, CancellationToken ct = default)
        {
            Announcement? announcement = await _announcementRepository.GetByIdAsync(announcementId, userId, null, null, ct) ?? throw new ObjectNotFoundException("Announcement Not Found!");

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
            announcement.IsPublished = false;
            await _announcementRepository.SaveChangesAsync(announcement, ct);
            return _mapper.Map<AnnouncementResponse>(announcement);
        }
        #endregion

        #region GetUsersAllAnnouncements
        public async Task<List<AnnouncementResponse>> GetUsersAllAnnouncements(Guid userId, bool isPublished, CancellationToken ct = default)
        {
            List<Announcement> announcements = await _announcementRepository.GetAllAsync(userId, isPublished, null, ct);
            return _mapper.Map<List<AnnouncementResponse>>(announcements);
        }
        #endregion

        #region GetUsersPagedAnnouncements
        public async Task<PagedResult<AnnouncementResponse>> GetUsersPagedAnnouncements(Guid userId, int page,
            int pageSize,
            bool isPublished,
            string? search = null,
            string? category = null,
            bool? isPinned = null, CancellationToken ct = default)
        {
            var announcements = await _announcementRepository.GetPagedAsync(page, pageSize, userId, isPublished, search, category, isPinned, null, ct);
            return new PagedResult<AnnouncementResponse>
            {
                Items = _mapper.Map<List<AnnouncementResponse>>(announcements.Items),
                TotalCount = announcements.TotalCount,
                PageSize = announcements.PageSize,
                CurrentPageNumber = announcements.CurrentPageNumber
            };
        }
        #endregion

        #region GetAnnouncementById
        public async Task<AnnouncementResponse> GetAnnouncementById(Guid userId, Guid announcementId, CancellationToken ct = default)
        {
            Announcement? announcement = await _announcementRepository.GetByIdAsync(announcementId, userId, null, null, ct) ?? throw new ObjectNotFoundException("Announcement Not Found!");
            return _mapper.Map<AnnouncementResponse>(announcement);
        }
        #endregion
    }
}
