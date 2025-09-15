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
        Task<AuthenticatedResponse?> Login(LoginRequest request);
        Task<AuthenticatedResponse> VerifyTwoFactorAuth(VerifyTwoFactorAuthRequest request);
        Task<AuthenticatedResponse> GenerateNewRefreshToken(RefreshTokenRequest request);
        Task ForgotPassword(ForgotPasswordRequest request);
        Task ResetPassword(string token, ResetPasswordRequest request);
    }
}
