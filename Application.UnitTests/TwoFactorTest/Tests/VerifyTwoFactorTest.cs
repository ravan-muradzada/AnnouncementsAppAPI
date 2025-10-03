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
    public class VerifyTwoFactorTest : TwoFactorTestBase
    {
        #region VerifyTwoFactorTest_Success
        [Fact]
        public async Task VerifyTwoFactorTest_Success()
        {
            _userManagerMock.Setup(um => um.VerifyTwoFactorTokenAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()));
            var exception = await Record.ExceptionAsync(() => _twoFactorService.VerifyTwoFactorCode(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<CancellationToken>()));
            Assert.Null(exception);
        }
        #endregion
    }
}
