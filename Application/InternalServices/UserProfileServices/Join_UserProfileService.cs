using Application.DTOs.Participant.Response;
using Application.InternalServiceInterfaces.IUserProfileServices;
using Domain.CustomExceptions;
using Domain.Entities;
using Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.InternalServices.UserProfileServices
{
    public class Join_UserProfileService : IJoin_UserProfileService
    {
        #region Fields
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IJoinRepository _joinRepository;
        #endregion

        #region Constructor
        public Join_UserProfileService(IAnnouncementRepository announcementRepository, IJoinRepository joinRepository)
        {
            _announcementRepository = announcementRepository;
            _joinRepository = joinRepository;
        }
        #endregion

        #region GetJoinedUsersList
        public async Task<List<JoinedUserResponse>> GetJoinedUsersList(Guid authorId, Guid announcementId, CancellationToken ct = default)
        {
            if(await _announcementRepository.ExistsAsync(announcementId, authorId, ct))
            {
                throw new ObjectNotFoundException("Announcement not found");
            }

            List<AnnouncementUser> announcementUsers = await _joinRepository.GetAnnouncementUsers(announcementId);
        
            return announcementUsers
                .Select(au => new JoinedUserResponse(
                    au.ApplicationUserId,
                    au.User.UserName!,
                    au.JoinedAt
                )).ToList();
        }
        #endregion

        #region RemoveUserFromGroup
        public async Task RemoveUserFromGroup(Guid authorId, Guid announcementId, Guid userId, CancellationToken ct = default)
        {
            if (await _announcementRepository.ExistsAsync(announcementId, authorId, ct))
            {
                throw new ObjectNotFoundException("Announcement not found");
            }

            AnnouncementUser? announcementUser = await _joinRepository.GetAnnouncementUserAsync(announcementId, userId) ?? throw new ObjectNotFoundException("User is not in this announcement!");

            await _joinRepository.LeaveAnnouncementAsync(announcementUser);
        }
        #endregion
    }
}
