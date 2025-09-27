using Application.DTOs.Auth.Request;
using Application.DTOs.Auth.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.InternalServiceInterfaces
{
    public interface IAuthService
    {
        Task<RegisterResponse> Register(RegisterRequest request);
        Task SendOtp(SendOtpRequest request);
        Task<AuthenticatedResponse> VerifyOtp(VerifyOtpRequest request);
        Task<AuthenticatedResponse?> Login(LoginRequest request, CancellationToken ct = default);
        Task<AuthenticatedResponse> VerifyTwoFactorAuth(VerifyTwoFactorAuthRequest request, CancellationToken ct = default);
        Task<AuthenticatedResponse> GenerateNewRefreshToken(RefreshTokenRequest request);
        Task ForgotPassword(ForgotPasswordRequest request, CancellationToken ct = default);
        Task ResetPassword(string token, ResetPasswordRequest request, CancellationToken ct = default);
        Task Logout(LogoutRequest request);
    }
}
