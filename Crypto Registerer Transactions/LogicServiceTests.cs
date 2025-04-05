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
        private Mock<ILogger<LogicService>> _logger;
        private Mock<IWalletIOService> _walletService;
        private LogicService _logicService;
        private Dictionary<string, double> _walletCache = new Dictionary<string, double>()
        {
            {"wallet1", 25 },
            {"wallet2",50 },
            {"wallet3",120 }
        };

        public LogicServiceTests()
        {
            _logger = new Mock<ILogger<LogicService>>();
            _walletService = new Mock<IWalletIOService>();
            _logicService = new LogicService(_logger.Object, _walletService.Object);
        }
        [Fact]
        public void IsWalletValid_ReturnsFalse_WhenWalletIsInvalid()
        {
            string invalidWallet = "TEST";
            bool result = _logicService.IsWalletValid(invalidWallet);
            Assert.False(result);
        }
        [Fact]
        public void IsWalletValid_ReturnsTrue_WhenWalletIsValid()
        {
            string validWallet = "2v8xdgPZPgZ8FZHqWhxHh2DiyTZKyxFiGFkmHvZPmbzL";
            bool result = _logicService.IsWalletValid(validWallet);
            Assert.True(result);
        }
        [Fact]
        public void IsWalletValid_ReturnsFalse_WhenWalletIsEmpty()
        {
            string emptyWallet = string.Empty;
            bool result = _logicService.IsWalletValid(emptyWallet);
            Assert.False(result);
        }
        [Fact]
        public void IsWalletValid_ReturnsFalse_WhenWalletIsNotBase58()
        {
            string exceptionWallet = "0OIl+/=ggggggghhhhtttttttrrrreeeeeeeeeewqweg";
            bool result = _logicService.IsWalletValid(exceptionWallet);
            Assert.False(result);
        }
        [Fact]
        public void SumOfTransactionsByWallet_ReturnsSum_WhenWalletExists()
        {
            _logicService.SetWalletSumsCache(_walletCache);
            double sum = _logicService.SumOfTransactionsByWallet("wallet2");
            Assert.Equal(sum, 50);
        }
        [Fact]
        public void SumOfTransactionsByWallet_ThrowsError_WhenWalletDoesNotExist()
        {
            _logicService.SetWalletSumsCache(_walletCache);
            Assert.Throws<KeyNotFoundException>(() => _logicService.SumOfTransactionsByWallet("wallet4"));
        }
    }
}
