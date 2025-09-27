using Application.InternalServiceInterfaces;
using Domain.CustomExceptions;
using Domain.Entities;
using Domain.RepositoryInterfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.InternalServices
{
    public class JoinService : IJoinService
    {
        #region Fields
        private readonly IJoinRepository _joinRepository;
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        #endregion

        #region Constructor
        public JoinService(IJoinRepository joinRepository, IAnnouncementRepository announcementRepository, UserManager<ApplicationUser> userManager)
        {
            _joinRepository = joinRepository;
            _announcementRepository = announcementRepository;
            _userManager = userManager;
        }
        #endregion

        #region JoinAnnouncementAsync
        public async Task JoinAnnouncementAsync(Guid announcementId, Guid userId, CancellationToken ct = default)
        {
            if (!(await _announcementRepository.ExistsAsync(announcementId, null, ct)) || (await _userManager.FindByIdAsync(userId.ToString()) is null))
            {
                throw new ObjectNotFoundException("User or announcement Not Found!");
            }

            if (await _joinRepository.CheckJoin(announcementId, userId, ct))
            {
                throw new ConflictException("User already joined this announcement.");
            }

            AnnouncementUser announcementUser = new AnnouncementUser
            {
                ApplicationUserId = userId,
                AnnouncementId = announcementId
            };
            await _joinRepository.JoinAnnouncementAsync(announcementUser, ct);
        }
        #endregion

        #region LeaveAnnouncementAsync
        public async Task LeaveAnnouncementAsync(Guid announcementId, Guid userId, CancellationToken ct = default)
        {
            if (!(await _announcementRepository.ExistsAsync(announcementId, null, ct)) || (await _userManager.FindByIdAsync(userId.ToString()) is null))
            {
                throw new ObjectNotFoundException("User or announcement Not Found!");
            }

            AnnouncementUser? announcementUser = await _joinRepository.GetAnnouncementUserAsync(announcementId, userId) ?? throw new ObjectNotFoundException("You did not join to this announcement!");

            await _joinRepository.LeaveAnnouncementAsync(announcementUser, ct);
        }
        #endregion
    }
}
