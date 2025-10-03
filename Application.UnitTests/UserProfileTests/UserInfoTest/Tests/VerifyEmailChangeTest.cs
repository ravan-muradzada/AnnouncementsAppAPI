using Application.DTOs.UserProfile.Request;
using Application.DTOs.UserProfile.Response;
using Domain.CustomExceptions;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using MockQueryable;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests.UserProfileTests.UserInfoTest.Tests
{
    public class VerifyEmailChangeTest : UserInfoTestBase
    {
        #region VerifyEmailChange_Success_ReturnsResponse
        [Fact]
        public async Task VerifyEmailChange_Success_ReturnsResponse()
        {
            Guid userId = Guid.NewGuid();
            VerifyEmailChangeRequest request = new VerifyEmailChangeRequest("123456");

            List<ApplicationUser> users = new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    Id = userId,
                    UserName = "u-1",
                    Email = "previous.1@gmail.com",
                },
                new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = "u-2",
                    Email = "just@gmail.com"
                }
            };

            var mockUser = users.BuildMock();
            _userManagerMock.Setup(u => u.Users).Returns(mockUser);
            _redisRepositoryMock.Setup(rp => rp.GetStringAsync($"email_change_candidate_{userId}"))
                .ReturnsAsync("candidate@gmail.com");
            _redisRepositoryMock.Setup(r => r.GetStringAsync($"email_change_otp_{userId}"))
            .ReturnsAsync("123456");

            var response = await _userInfoService.VerifyEmailChange(userId, request, default);

            Assert.IsType<UserProfileResponse>(response);
            Assert.Equal(users[0].UserName, response.UserName);
            Assert.Equal("candidate@gmail.com", response.Email);
            Assert.Equal("candidate@gmail.com", users[0].Email);
        }
        #endregion

        #region UserNotFound
        [Fact]
        public async Task VerifyEmailChange_UserNotFound_ThrowsObjectNotFoundException()
        {
            Guid userId = Guid.NewGuid();
            VerifyEmailChangeRequest request = new VerifyEmailChangeRequest("1234");

            List<ApplicationUser> users = new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = "u-1",
                    Email = "previous.1@gmail.com",
                },
                new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = "u-2",
                    Email = "just@gmail.com"
                }
            };

            var mockUser = users.BuildMock();
            _userManagerMock.Setup(u => u.Users).Returns(mockUser);
            _redisRepositoryMock.Setup(r => r.GetStringAsync($"email_change_otp_{userId}"))
                     .ReturnsAsync("123456"); // mock OTP
            _redisRepositoryMock.Setup(r => r.GetStringAsync($"email_change_candidate_{userId}"))
                     .ReturnsAsync("test@example.com");

            await Assert.ThrowsAsync<ObjectNotFoundException>(() => _userInfoService.VerifyEmailChange(userId, request, default));
        }
        #endregion

        #region VerifyEmailChange_EmailCandidateNull_ThrowsObjectNotFoundException
        [Fact]
        public async Task VerifyEmailChange_EmailCandidateNull_ThrowsObjectNotFoundException()
        {
            Guid userId = Guid.NewGuid();
            VerifyEmailChangeRequest request = new VerifyEmailChangeRequest("1234");

            List<ApplicationUser> users = new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    Id = userId,
                    UserName = "u-1",
                    Email = "previous.1@gmail.com",
                },
                new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = "u-2",
                    Email = "just@gmail.com"
                }
            };

            var mockUser = users.BuildMock();
            _userManagerMock.Setup(u => u.Users).Returns(mockUser);
            _redisRepositoryMock.Setup(r => r.GetStringAsync($"email_change_otp_{userId}"))
                     .ReturnsAsync("123456"); // mock OTP
            _redisRepositoryMock.Setup(r => r.GetStringAsync($"email_change_candidate_{userId}"));

            await Assert.ThrowsAsync<ObjectNotFoundException>(() => _userInfoService.VerifyEmailChange(userId, request, default));
        }
        #endregion

        #region VerifyEmailChange_CachedOtpNull_ThrowsObjectNotFoundException
        [Fact]
        public async Task VerifyEmailChange_CachedOtpNull_ThrowsObjectNotFoundException()
        {
            Guid userId = Guid.NewGuid();
            VerifyEmailChangeRequest request = new VerifyEmailChangeRequest("1234");

            List<ApplicationUser> users = new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    Id = userId,
                    UserName = "u-1",
                    Email = "previous.1@gmail.com",
                },
                new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = "u-2",
                    Email = "just@gmail.com"
                }
            };

            var mockUser = users.BuildMock();
            _userManagerMock.Setup(u => u.Users).Returns(mockUser);
            _redisRepositoryMock.Setup(r => r.GetStringAsync($"email_change_otp_{userId}"));
            _redisRepositoryMock.Setup(r => r.GetStringAsync($"email_change_candidate_{userId}"))
                .ReturnsAsync("candidate@gmail.com");


            await Assert.ThrowsAsync<InvalidCredentialsException>(() => _userInfoService.VerifyEmailChange(userId, request, default));
        }
        #endregion

        #region VerifyEmailChange_CachedOtpMismatch_ThrowsObjectNotFoundException
        [Fact]
        public async Task VerifyEmailChange_CachedOtpMismatch_ThrowsObjectNotFoundException()
        {
            Guid userId = Guid.NewGuid();
            VerifyEmailChangeRequest request = new VerifyEmailChangeRequest("1234");

            List<ApplicationUser> users = new List<ApplicationUser>()
            {
                new ApplicationUser
                {
                    Id = userId,
                    UserName = "u-1",
                    Email = "previous.1@gmail.com",
                },
                new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = "u-2",
                    Email = "just@gmail.com"
                }
            };

            var mockUser = users.BuildMock();
            _userManagerMock.Setup(u => u.Users).Returns(mockUser);
            _redisRepositoryMock.Setup(r => r.GetStringAsync($"email_change_otp_{userId}"))
                .ReturnsAsync("12345");
            _redisRepositoryMock.Setup(r => r.GetStringAsync($"email_change_candidate_{userId}"))
                .ReturnsAsync("candidate@gmail.com");


            await Assert.ThrowsAsync<InvalidCredentialsException>(() => _userInfoService.VerifyEmailChange(userId, request, default));
        }
        #endregion
    }
}
