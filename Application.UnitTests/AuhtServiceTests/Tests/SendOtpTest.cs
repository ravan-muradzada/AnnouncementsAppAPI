using Application.DTOs.Auth.Request;
using Domain.CustomExceptions;
using Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests.AuhtServiceTests.Tests
{
    public class SendOtpTest : AuthServiceTestBase
    {
        #region Success
        /// <summary>
        /// Checks the success case
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task SendOtp_Success_ReturnsResponse()
        {
            SendOtpRequest request = new SendOtpRequest
            {
                Email = "ravan@gmail.com"
            };

            _userManagerMock.Setup(um => um.FindByEmailAsync(request.Email))
                .ReturnsAsync(new ApplicationUser { Email = request.Email });
            _userManagerMock.Setup(um => um.UpdateAsync(It.IsAny<ApplicationUser>()));
            _emailServiceMock.Setup(es => es.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            var response = await Record.ExceptionAsync(() => _authService.SendOtp(request));
            Assert.Null(response);
        }
        #endregion

        #region NullUserCase
        /// <summary>
        ///  Returns ObjectNotFoundException when a user is not found!
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task SendOtp_NullUser_ObjectNotFoundException()
        {
            SendOtpRequest request = new SendOtpRequest
            {
                Email = "ravan@gmail.com"
            };

            _userManagerMock.Setup(um => um.FindByEmailAsync(request.Email));
            await Assert.ThrowsAsync<ObjectNotFoundException>(() => _authService.SendOtp(request));
        }
        #endregion
    }
}
