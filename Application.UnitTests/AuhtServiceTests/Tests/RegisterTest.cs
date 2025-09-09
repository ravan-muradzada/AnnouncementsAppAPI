using Application.DTOs.Auth.Request;
using Domain.CustomExceptions;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests.AuhtServiceTests.Tests
{
    public class RegisterTest : AuthServiceTestBase
    {

        #region Success
        /// <summary>
        /// This checks the success case.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RegisterAsync_Success_ReturnsResponse()
        {
            RegisterRequest request = new RegisterRequest
            {
                UserName = "MrRavan",
                Email = "ravan@gmail.com",
                Password = "Ravans_password123!"
            };

            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), request.Password))
                .ReturnsAsync(IdentityResult.Success);

            var response = await _authService.Register(request);

            Assert.Equal("ravan@gmail.com", response.Email);
        }
        #endregion

        #region ErrorDuplicateEmail
        /// <summary>
        /// Returns ConflictException when an email is duplicated!
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RegisterAsync_DuplicateEmail_ThrowsConflictException()
        {
            RegisterRequest request = new RegisterRequest
            {
                UserName = "_",
                Email = "duplicate@gmail.com",
                Password = "Ravans_password123!"
            };

            var identityResult = IdentityResult.Failed(new IdentityError
            {
                Code = "DuplicateEmail",
                Description = "Email already taken"
            });

            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), request.Password))
                .ReturnsAsync(identityResult);

            await Assert.ThrowsAsync<ConflictException>(() => _authService.Register(request));
        }
        #endregion

        #region PasswordTooShort
        /// <summary>
        /// It returns ValidationException when provided password is short
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RegisterAsync_PasswordTooShort_ThrowsValidationException()
        {
            RegisterRequest request = new RegisterRequest
            {
                UserName = "_",
                Email = "ravan@gmail.com",
                Password = "_"
            };

            var identityResult = IdentityResult.Failed(new IdentityError
            {
                Code = "PasswordTooShort",
                Description = "Too short"
            });

            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), request.Password))
                .ReturnsAsync(identityResult);

            await Assert.ThrowsAsync<ValidationException>(() => _authService.Register(request));
        }
        #endregion

        #region InternalServerException
        /// <summary>
        /// For other errors, it should return InternalServerException
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RegisterAsync_OtherErrors_InternalServerException()
        {
            RegisterRequest request = new RegisterRequest
            {
                UserName = "_",
                Email = "ravan@gmail.com",
                Password = "ravan12345_R"
            };

            var identityResult = IdentityResult.Failed(new IdentityError
            {
                Code = "OtherErrors",
                Description = "It_might_server_related_error..."
            });

            _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), request.Password))
                .ReturnsAsync(identityResult);

            await Assert.ThrowsAsync<InternalServerException>(() => _authService.Register(request));
        }
        #endregion
    }
}
