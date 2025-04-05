using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Crypto_Registerer_Transactions
{
    public class WalletIOService : IWalletIOService
    {
        private ILogger<WalletIOService> _logger;
        private IConsoleService _console;
        public WalletIOService(ILogger<WalletIOService> logger, IConsoleService consoleService)
        {
            _logger = logger;
            _console = consoleService;
        }
        /// <summary>
        /// Saves transaction registration in wallets.txt
        /// </summary>
        /// <param name="wallet">Wallet's address</param>
        /// <param name="_walletSumsCache">Cache of the wallets, loaded at the start of the program</param>
        /// <param name="sum">Sum of the transaction</param>
        public void SaveTransaction(string wallet, Dictionary<string, double> _walletSumsCache, double sum)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(@"wallets.txt",true))
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
        /// <summary>
        /// Loads the wallets and their sums into a dictionary
        /// </summary>
        /// <returns>Dictionary filled with wallets and their sums</returns>
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
