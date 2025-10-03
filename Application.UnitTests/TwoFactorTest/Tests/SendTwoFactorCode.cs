using Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests.TwoFactorTest.Tests
{
    public class SendTwoFactorCode : TwoFactorTestBase
    {
        #region SendTwoFactor_Success
        [Fact]
        public async Task SendTwoFactor_Success()
        {
            _userManagerMock.Setup(um => um.GenerateTwoFactorTokenAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(It.IsAny<string>());
            _emailServiceMock.Setup(es => es.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            var exception = await Record.ExceptionAsync(() => _twoFactorService.SendTwoFactorCode(new ApplicationUser { Email = "email@gmail.com" }, default));
            Assert.Null(exception);
        }
        #endregion
    }
}
