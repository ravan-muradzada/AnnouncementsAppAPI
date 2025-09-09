using Application.DTOs.Auth.Request;
using Application.DTOs.Auth.Response;
using Application.ExternalServiceInterfaces;
using Application.InternalServiceInterfaces;
using Domain.CustomExceptions;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Security.Authentication;
using System.Text.Encodings.Web;

namespace Application.InternalServices
{
    public class AuthService : IAuthService
    {
        #region Fields
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IAccessTokenService _accessTokenService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly ITwoFactorService _twoFactorService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Constructor
        public AuthService(UserManager<ApplicationUser> userManager, IEmailService emailService, IAccessTokenService accessTokenService, IRefreshTokenService refreshTokenService, ITwoFactorService twoFactorService, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _emailService = emailService;
            _accessTokenService = accessTokenService;
            _refreshTokenService = refreshTokenService;
            _twoFactorService = twoFactorService;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region GenerateResetLink
        private string GenerateResetLink(string token, string email)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var scheme = request.Scheme;
            var host = request.Host.Value;
            return $"{scheme}://{host}/Account/ResetPassword?token={token}&email={email}";
        }
        #endregion

        #region Register
        public async Task<RegisterResponse> Register(RegisterRequest request)
        {
            ApplicationUser user = new ApplicationUser { UserName = request.UserName, Email = request.Email, EmailConfirmed = false };

            IdentityResult result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                IdentityError? error = result.Errors.FirstOrDefault();

                if (error is null)
                {
                    throw new InternalServerException();
                }

                switch (error.Code)
                {
                    case "DuplicateUserName":
                    case "DuplicateEmail":
                        throw new ConflictException(error.Description);

                    case "PasswordTooShort":
                    case "PasswordRequiresNonAlphanumeric":
                    case "InvalidUserName":
                        throw new ValidationException(error.Description);

                    default:
                        throw new InternalServerException(error.Description);
                }
            }

            RegisterResponse response = new RegisterResponse { Email = request.Email };

            return response;
        }
        #endregion

        #region SendOtp
        public async Task SendOtp(SendOtpRequest request)
        {
            string to = request.Email;
            var user = await _userManager.FindByEmailAsync(to);

            if (user is null)
            {
                throw new ObjectNotFoundException("User not found!");
            }

            string otp = new Random().Next(100000, 999999).ToString();

            string body = $"<div style=\"font-family:sans-serif;text-align:center;padding:20px;background:#f4f4f4;border-radius:10px;max-width:400px;margin:20px auto;\">\r\n  <h2 style=\"color:#333;margin-bottom:15px;\">Your One-Time Password:</h2>\r\n  <div style=\"font-size:28px;color:#1a73e8;font-weight:bold;background:#e8f0fe;padding:10px 20px;border-radius:8px;display:inline-block;\">\r\n    {otp}\r\n  </div>\r\n</div>\r\n";

            user.OTP = otp;
            user.OTPExpirationTime = DateTime.UtcNow.AddMinutes(5);
            await _userManager.UpdateAsync(user);

            await _emailService.SendEmail(to, "OTP Sent!", body);
        }
        #endregion

        #region VerifyOtp
        public async Task<AuthenticatedResponse> VerifyOtp(VerifyOtpRequest request)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                throw new ObjectNotFoundException("User not found!");
            }

            if (!string.Equals(user.OTP, request.Otp) || user.OTPExpirationTime < DateTime.UtcNow)
            {
                throw new InvalidCredentialsException("Invalid or expired OTP!");
            }

            string accessToken = _accessTokenService.GenerateToken(user);
            RefreshToken refreshTokenObject = await _refreshTokenService.GenerateRefreshToken(user.Id);

            user.OTP = null;
            user.OTPExpirationTime = null;
            user.EmailConfirmed = true;
            user.RefreshTokens.Add(refreshTokenObject);
            await _userManager.UpdateAsync(user);

            return new AuthenticatedResponse { AccessToken = accessToken, RefreshToken = refreshTokenObject.Token };
        }
        #endregion

        #region Login
        public async Task<AuthenticatedResponse?> Login(LoginRequest request)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(request.Username);
            if (user is null)
            {
                throw new InvalidCredentialsException("Either username or password is invalid!");
            }

            bool checkPassword = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!checkPassword)
            {
                throw new InvalidCredentialsException("Either username or password is invalid!");
            }

            if (!user.EmailConfirmed)
            {
                throw new NotConfirmedException("Email not confirmed!");
            }

            if (await _userManager.GetTwoFactorEnabledAsync(user))
            {
                await _twoFactorService.SendTwoFactorCode(user);
                return null;
            }

            string accessToken = _accessTokenService.GenerateToken(user);
            RefreshToken refreshTokenObject = await _refreshTokenService.GenerateRefreshToken(user.Id);

            user.RefreshTokens.Add(refreshTokenObject);
            await _userManager.UpdateAsync(user);

            return new AuthenticatedResponse { AccessToken = accessToken, RefreshToken = refreshTokenObject.Token };
        }
        #endregion

        #region VerifyTwoFactorAuth
        public async Task<AuthenticatedResponse> VerifyTwoFactorAuth(VerifyTwoFactorAuthRequest request)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null) throw new ObjectNotFoundException("User not found!");

            if (!await _twoFactorService.VerifyTwoFactorCode(user, request.Code))
            {
                throw new TwoFactorAuthFailedException("The provided code is wrong!");
            }

            string accessToken = _accessTokenService.GenerateToken(user);
            RefreshToken refreshTokenObject = await _refreshTokenService.GenerateRefreshToken(user.Id);

            user.RefreshTokens.Add(refreshTokenObject);
            await _userManager.UpdateAsync(user);

            return new AuthenticatedResponse { AccessToken = accessToken, RefreshToken = refreshTokenObject.Token };
        }
        #endregion

        #region GenerateNewRefreshToken
        public async Task<AuthenticatedResponse> GenerateNewRefreshToken(RefreshTokenRequest request)
        {
            string refreshToken = request.RefreshToken;

            ApplicationUser? user = await _refreshTokenService.FindUserOfRefreshTokenAndDeleteToken(refreshToken);

            if (user is null)
            {
                throw new InvalidCredentialsException("Invalid or expired refresh token!");
            }

            string newAccessToken = _accessTokenService.GenerateToken(user);
            RefreshToken refreshTokenObject = await _refreshTokenService.GenerateRefreshToken(user.Id);
            user.RefreshTokens.Add(refreshTokenObject);
            await _userManager.UpdateAsync(user);

            return new AuthenticatedResponse { AccessToken = newAccessToken, RefreshToken = refreshTokenObject.Token };
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

        #region ForgotPassword
        public async Task ForgotPassword(ForgotPasswordRequest request)
        {

            ApplicationUser? user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null) throw new ObjectNotFoundException("User not found!");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = GenerateResetLink(token, request.Email);

            await _emailService.SendEmail(request.Email, "Reset Password Link", resetLink);
        }
        #endregion

        #region ResetPassword
        public async Task ResetPassword(string token, ResetPasswordRequest request)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null) throw new ObjectNotFoundException("User not found!");

            IdentityResult result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
            await _refreshTokenService.InvalidateUserTokensAsync(user.Id);
            if (!result.Succeeded)
            {
                throw new ValidationException(string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
        #endregion
    }
}
