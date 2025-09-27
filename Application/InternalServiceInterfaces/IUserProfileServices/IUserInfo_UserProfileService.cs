using Application.DTOs.UserProfile.Request;
using Application.DTOs.UserProfile.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.InternalServiceInterfaces.IUserProfileServices
{
    public interface IUserInfo_UserProfileService
    {
        Task<UserProfileResponse> GetUser(Guid userId, CancellationToken ct = default);
        Task<UserProfileResponse> ChangeUsername(Guid userId, ChangeUsernameRequest request, CancellationToken ct = default);
        Task ChangeEmail(Guid userId, ChangeEmailRequest request, CancellationToken ct = default);
        Task<UserProfileResponse> VerifyEmailChange(Guid userId, VerifyEmailChangeRequest request, CancellationToken ct = default);
        Task ChangePassword(Guid userId, ChangePasswordRequest request, CancellationToken ct = default);
    }
}
