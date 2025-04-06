using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Crypto_Registerer_Transactions.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Crypto_Registerer_Transactions.Tests
{
    public class ConsoleServiceTests
    {
        private Mock<ILogger<ConsoleService>> _logger;
        private ConsoleService _service;
        private StringWriter sw = new StringWriter();
        private Dictionary<string, double> _walletCache = new Dictionary<string, double>()
        {
            {"wallet1", 25 },
            {"wallet2",50 },
            {"wallet3",120 }
        };
        public ConsoleServiceTests()
        {
            _logger = new Mock<ILogger<ConsoleService>>();
            _service = new ConsoleService(_logger.Object);
        }
        [Fact]
        public void SayMessage_OutputToConsole()
        {
            Console.SetOut(sw);
            _service.SayMessage("TEST", ConsoleColor.Red);
            string output = sw.ToString().Trim();
            Assert.Equal("TEST", output);
        }
        [Fact]
        public void ShowList_OutputToConsole()
        {
            Console.SetOut(sw);
            _service.ShowList(_walletCache);
            string output = sw.ToString().Trim();
            Assert.Contains("wallet1", output);
            Assert.Contains(25.ToString(), output);
        }
        [Fact]
        public void GetSum_ReturnsTrue_WhenTextMatches()
        {
            StringReader sr = new StringReader("25");
            Console.SetIn(sr);
            var result = _service.GetSum();
            Assert.Equal(25, result);
        }
    }
}
