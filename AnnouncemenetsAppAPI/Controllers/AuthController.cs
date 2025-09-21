using AnnouncemenetsAppAPI.Extensions;
using Application.DTOs.Auth.Request;
using Application.DTOs.Auth.Response;
using Application.InternalServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnnouncemenetsAppAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region Fields
        private readonly IAuthService _authService;
        #endregion

        #region Constructor
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        #endregion

        #region Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            RegisterResponse response = await _authService.Register(request);
            return Ok(response);
        }
        #endregion

        #region SendOtp
        [HttpPost]
        public async Task<IActionResult> SendOtp(SendOtpRequest request)
        {
            await _authService.SendOtp(request);
            return Ok(new
            {
                Message = "Otp has been sent to email!"
            });
        }
        #endregion

        #region VerifyOtp
        [HttpPost]
        public async Task<IActionResult> VerifyOtp(VerifyOtpRequest request)
        {
            AuthenticatedResponse response = await _authService.VerifyOtp(request);
            return Ok(response);
        }
        #endregion

        #region Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            AuthenticatedResponse? response = await _authService.Login(request);
            if (response is null)
            {
                return Ok(new
                {
                    Message = "You need to pass the second factor in 2FA!"
                });
            }

            return Ok(response);
        }
        #endregion

        #region VerifyTwoFactorAuth
        [HttpPost]
        public async Task<IActionResult> VerifyTwoFactorAuth(VerifyTwoFactorAuthRequest request)
        {
            AuthenticatedResponse response = await _authService.VerifyTwoFactorAuth(request);
            return Ok(response);
        }
        #endregion

        #region GenerateNewRefreshToken
        [HttpPost]
        public async Task<IActionResult> GenerateNewRefreshToken(RefreshTokenRequest request)
        {
            AuthenticatedResponse response = await _authService.GenerateNewRefreshToken(request);
            return Ok(response);
        }
        #endregion

        #region ForgotPassword
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
        {
            await _authService.ForgotPassword(request);
            return Ok(new
            {
                Message = "Email is sent, check your inbox!"
            });
        }
        #endregion

        #region ResetPassword
        [HttpPut("{token}")]
        public async Task<IActionResult> ResetPassword(string token, ResetPasswordRequest request)
        {
            await _authService.ResetPassword(token, request);
            return Ok(new
            {
                Message = "Your password is successfully reset!"
            });
        }
        #endregion

        #region Logout
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout(LogoutRequest request)
        {
            Guid userId = User.GetUserId();
            await _authService.Logout(request);
            return Ok(new
            {
                Message = "You are successfully logged out!"
            });
        }
        #endregion
    }
}
