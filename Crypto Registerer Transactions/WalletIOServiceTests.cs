using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Solnet.Wallet;
using Xunit;

namespace Crypto_Registerer_Transactions
{
    public class WalletIOServiceTests
    {
        private Mock<ILogger<WalletIOService>> _logger;
        private Mock<IConsoleService> _console;
        private WalletIOService _service;
        private Dictionary<string,double> walletSumsCache = new Dictionary<string, double>();
        public WalletIOServiceTests()
        {
            _logger = new Mock<ILogger<WalletIOService>>();
            _console = new Mock<IConsoleService>();
            _service = new WalletIOService(_logger.Object,_console.Object);
        }
        [Fact]
        public void SaveTransaction_WritesToDict()
        {
            _service.SaveTransaction("wallet", walletSumsCache, 20);
            Assert.Single(walletSumsCache);
            Assert.Equal(20, walletSumsCache["wallet"]);
        }
        [Fact]
        public async Task LoadTransactionDataAsync_LoadsCache()
        {
            var filePath = @"wallets.txt";
            File.WriteAllText(filePath, "test_wallet\n10\n");
            walletSumsCache = await _service.LoadTransactionDataAsync();
            Assert.Equal(10, walletSumsCache["test_wallet"]);
        }
    }
}
