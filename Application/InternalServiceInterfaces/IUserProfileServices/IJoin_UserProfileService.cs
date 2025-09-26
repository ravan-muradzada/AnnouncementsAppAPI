using Application.DTOs.Participant.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.InternalServiceInterfaces.IUserProfileServices
{
    public interface IJoin_UserProfileService
    {
        Task<List<JoinedUserResponse>> GetJoinedUsersList(Guid authorId, Guid announcementId, CancellationToken ct = default);
        Task RemoveUserFromGroup(Guid authorId, Guid announcementId, Guid userId, CancellationToken ct = default);   
    }
}
