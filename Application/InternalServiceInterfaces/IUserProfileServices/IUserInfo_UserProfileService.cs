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
        Task<UserProfileResponse> GetUser(Guid userId);
        Task<UserProfileResponse> ChangeUsername(Guid userId, ChangeUsernameRequest request);
        Task ChangeEmail(Guid userId, ChangeEmailRequest request);
        Task<UserProfileResponse> VerifyEmailChange(Guid userId, VerifyEmailChangeRequest request);
        Task ChangePassword(Guid userId, ChangePasswordRequest request);
    }
}
