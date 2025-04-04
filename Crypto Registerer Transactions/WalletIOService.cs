using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Crypto_Registerer_Transactions
{
    internal class WalletIOService : IWalletIOService
    {
        private ILogger<WalletIOService> _logger;
        private IConsoleService _console;
        public WalletIOService(ILogger<WalletIOService> logger, IConsoleService consoleService)
        {
            _logger = logger;
            _console = consoleService;
        }
        public void SaveTransaction(string wallet, Dictionary<string, double> _walletSumsCache, double sum)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(@"wallets.txt"))
                {
                    sw.WriteLine(wallet);
                    sw.WriteLine(sum);
                }
                if (_walletSumsCache.TryGetValue(wallet, out double value))
                {
                    _walletSumsCache[wallet] = value + sum;
                }
                else
                {
                    _walletSumsCache[wallet] = sum;
                }
                _console.SayMessage("Transaction saved successfully!", ConsoleColor.Green);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error came from {nameof(SaveTransaction)}");
                throw;
            }
        }
        public async Task<Dictionary<string, double>> LoadTransactionDataAsync()
        {
            try
            {
                Dictionary<string, double> _walletSumsCache = new();
                var lines = await File.ReadAllLinesAsync(@"wallets.txt");
                for (int i = 0; i < lines.Length; i += 2)
                {
                    string wallet = lines[i];
                    if (double.TryParse(lines[i + 1], out double sum))
                    {
                        if (_walletSumsCache.TryGetValue(wallet, out double value))
                        {
                            _walletSumsCache[wallet] = value + sum;
                        }
                        else
                        {
                            _walletSumsCache[wallet] = sum;
                        }
                    }
                }
                return _walletSumsCache;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error came from {nameof(LoadTransactionDataAsync)}");
                throw;
            }
        }
    }
}
