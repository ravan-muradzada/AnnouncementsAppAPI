using Application.DTOs.Announcement.Request;
using Application.DTOs.Announcement.Response;
using Application.DTOs.UserProfile.Request;
using Application.DTOs.UserProfile.Response;
using Application.ExternalServiceInterfaces;
using Application.InternalServiceInterfaces;
using AutoMapper;
using Domain.Common;
using Domain.CustomExceptions;
using Domain.Entities;
using Domain.RepositoryInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.InternalServices
{
    public class UserProfileService : IUserProfileService
    {
        #region Fields
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IRedisRepository _redisRepository;
        private readonly IRefreshTokenService _refreshTokenService;
        #endregion

        #region Constructor
        public UserProfileService(UserManager<ApplicationUser> userManager, IAnnouncementRepository announcementRepository ,IMapper mapper, IEmailService emailService, IRedisRepository redisRepository, IRefreshTokenService refreshTokenService)
        {
            _userManager = userManager;
            _announcementRepository = announcementRepository;
            _mapper = mapper;
            _emailService = emailService;
            _redisRepository = redisRepository;
            _refreshTokenService = refreshTokenService;
        }
        #endregion

        #region GetUser
        public async Task<UserProfileResponse> GetUser(Guid userId)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null) throw new ObjectNotFoundException("User not found");
            return _mapper.Map<UserProfileResponse>(user);
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

        #region UpdateAnnouncement
        public async Task<AnnouncementResponse> UpdateAnnouncement(Guid userId, Guid announcementId, UpdateAnnouncementRequest request, CancellationToken ct = default)
        {
            Announcement? announcement = await _announcementRepository.GetByIdAsync(announcementId, userId, null, ct) ?? throw new ObjectNotFoundException("Announcement Not Found!");

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
            List<Announcement> announcements = await _announcementRepository.GetAllAsync(userId, isPublished, ct);
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
            var announcements = await _announcementRepository.GetPagedAsync(page, pageSize, userId, isPublished, search, category, isPinned, ct);
            return _mapper.Map<PagedResult<AnnouncementResponse>>(announcements);
        }
        #endregion

        #region GetAnnouncementById
        public async Task<AnnouncementResponse> GetAnnouncementById(Guid userId, Guid announcementId, CancellationToken ct = default)
        {
            Announcement? announcement = await _announcementRepository.GetByIdAsync(announcementId, userId, null, ct) ?? throw new ObjectNotFoundException("Announcement Not Found!");
            return _mapper.Map<AnnouncementResponse>(announcement);
        }
        #endregion

        #region ChangeEmail
        public async Task ChangeEmail(Guid userId, ChangeEmailRequest request)
        {
            string to = request.NewEmail;
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null)
            {
                throw new ObjectNotFoundException("User not found!");
            }

            string otp = new Random().Next(100000, 999999).ToString();

            string body = $"<div style=\"font-family:sans-serif;text-align:center;padding:20px;background:#f4f4f4;border-radius:10px;max-width:400px;margin:20px auto;\">\r\n  <h2 style=\"color:#333;margin-bottom:15px;\">Your One-Time Password:</h2>\r\n  <div style=\"font-size:28px;color:#1a73e8;font-weight:bold;background:#e8f0fe;padding:10px 20px;border-radius:8px;display:inline-block;\">\r\n    {otp}\r\n  </div>\r\n</div>\r\n";

            await _redisRepository.SetStringAsync($"email_change_otp_{user.Id}", otp, TimeSpan.FromMinutes(5));
            await _redisRepository.SetStringAsync($"email_change_candidate_{user.Id}", to, TimeSpan.FromMinutes(5));
            await _userManager.UpdateAsync(user);

            await _emailService.SendEmail(to, "OTP Sent!", body);
        }
        #endregion

        #region VerifyEmailChange
        public async Task<UserProfileResponse> VerifyEmailChange(Guid userId, VerifyEmailChangeRequest request)
        {
            ApplicationUser? user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            string? cachedOtp = await _redisRepository.GetStringAsync($"email_change_otp_{userId}");
            string? cachedEmailCandidate = await _redisRepository.GetStringAsync($"email_change_candidate_{userId}");

            if (user is null || cachedEmailCandidate is null)
            {
                throw new ObjectNotFoundException("User or New Email Candidate Not Found!");
            }

            if (cachedOtp is null || !string.Equals(cachedOtp, request.OTP))
            {
                throw new InvalidCredentialsException("Invalid or expired OTP!");
            }

            user.Email = cachedEmailCandidate;
            await _redisRepository.DeleteAsync($"email_change_otp_{userId}");
            await _redisRepository.DeleteAsync($"email_change_candidate_{userId}");
            await _userManager.UpdateAsync(user);

            return _mapper.Map<UserProfileResponse>(user);
        }
        #endregion

        #region ChangePassword
        public async Task ChangePassword(Guid userId, ChangePasswordRequest request)
        {
            string currentPassword = request.CurrentPassword;
            ApplicationUser? user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null) throw new ObjectNotFoundException("User Not Found!");

            var isCurrentPasswordValid = await _userManager.CheckPasswordAsync(user, currentPassword);
            if (!isCurrentPasswordValid) throw new InvalidCredentialsException("Current password is incorrect!");

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, request.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new IdentityException($"Password change failed: {errors}");
            }
            await _refreshTokenService.InvalidateUserTokensAsync(userId);
            await _userManager.UpdateSecurityStampAsync(user);
        }
        #endregion

        #region ChangeUsername
        public async Task<UserProfileResponse> ChangeUsername(Guid userId, ChangeUsernameRequest request)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null) throw new ObjectNotFoundException("User Not Found!");

            string newUsername = request.NewUsername;

            if (string.IsNullOrEmpty(newUsername)) throw new NullParameterException("Username cannot be null or empty!");

            ApplicationUser? existingUser = await _userManager.FindByNameAsync(newUsername);
            if (existingUser is not null) throw new ConflictException("Username already taken!");

            if (user.LimitDate is null || user.LimitDate <= DateTime.UtcNow)
            {
                user.LimitDate = DateTime.UtcNow.AddMonths(1);
                user.currentCountOfChange = 0;
            }

            if (user.currentCountOfChange >= user.countOfAllowedChangeUsername)
            {
                throw new NotAllowedException("You can change username only 3 times in a month!");
            }

            user.currentCountOfChange++;
            user.UserName = newUsername;

            await _userManager.UpdateAsync(user);

            return _mapper.Map<UserProfileResponse>(user);
        }
        #endregion

        #region EnableTwoFactorAuth 
        public async Task EnableTwoFactorAuth(Guid userId)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null) throw new ObjectNotFoundException("User not found!");
            await _userManager.SetTwoFactorEnabledAsync(user, true);
        }
        #endregion

        #region DisableTwoFactorAuth
        public async Task DisableTwoFactorAuth(Guid userId)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null) throw new ObjectNotFoundException("User not found!");
            await _userManager.SetTwoFactorEnabledAsync(user, false);
        }
        #endregion
    }
}
