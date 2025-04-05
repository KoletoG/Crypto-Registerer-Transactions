using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
namespace Crypto_Registerer_Transactions
{
    public class LogicServiceTests
    {
        [Fact]
        public void IsWalletValid_ReturnsFalse_WhenWalletIsInvalid()
        {
            var logger = new Mock<ILogger<LogicService>>();
            var walletService = new Mock<IWalletIOService>();
            var logicService= new LogicService(logger.Object, walletService.Object);
            string invalidWallet = "TEST";
            bool result = logicService.IsWalletValid(invalidWallet);
            Assert.False(result);
        }
        [Fact]
        public void IsWalletValid_ReturnsTrue_WhenWalletIsValid()
        {
            var logger = new Mock<ILogger<LogicService>>();
            var walletService = new Mock<IWalletIOService>();
            var logicService = new LogicService(logger.Object, walletService.Object);
            string validWallet = "2v8xdgPZPgZ8FZHqWhxHh2DiyTZKyxFiGFkmHvZPmbzL";
            bool result = logicService.IsWalletValid(validWallet);
            Assert.True(result);
        }
        [Fact]
        public void IsWalletValid_ReturnsFalse_WhenWalletIsEmpty()
        {
            var logger = new Mock<ILogger<LogicService>>();
            var walletService = new Mock<IWalletIOService>();
            var logicService = new LogicService(logger.Object, walletService.Object);
            string emptyWallet = string.Empty;
            bool result = logicService.IsWalletValid(emptyWallet);
            Assert.False(result);
        }
        [Fact]
        public void IsWalletValid_ReturnsFalse_WhenWalletIsNotBase58()
        {
            var logger = new Mock<ILogger<LogicService>>();
            var walletService = new Mock<IWalletIOService>();
            var logicService = new LogicService(logger.Object, walletService.Object);
            string exceptionWallet = "0OIl+/=";
            bool result = logicService.IsWalletValid(exceptionWallet);
            Assert.False(result);
        }
    }
}
