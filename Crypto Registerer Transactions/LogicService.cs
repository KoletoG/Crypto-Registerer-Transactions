using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Solnet.Wallet;
namespace Crypto_Registerer_Transactions
{
    public class LogicService : ILogicService
    {
        private ILogger<LogicService> _logger;
        private Dictionary<string, double> _walletSumsCache;
        private IWalletIOService _walletIOService;
        public LogicService(ILogger<LogicService> logger, IWalletIOService walletIOService)
        {
            _logger = logger;
            _walletIOService = walletIOService;
        }
        public void SetWalletSumsCache(Dictionary<string, double> walletSumsCache)
        {
            _walletSumsCache = walletSumsCache;
        }
        /// <summary>
        /// Gets the sum of the wallet's registered transactions
        /// </summary>
        /// <param name="wallet">Wallet's address</param>
        /// <returns>Sum of wallet's registered transactions</returns>
        public double SumOfTransactionsByWallet(string wallet)
        {
            try
            {
                return _walletSumsCache[wallet];
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, $"Tries to get sum from wallet which doesn't exist. Source: {nameof(SumOfTransactionsByWallet)}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error came from {nameof(SumOfTransactionsByWallet)}");
                throw;
            }
        }
        public Dictionary<string, double> GetWalletsCache()
        {
            try
            {
                return _walletSumsCache;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error came from {nameof(GetWalletsCache)}");
                throw;
            }
        }
        public async Task InitializeAsync()
        {
            try
            {
                _walletSumsCache = await _walletIOService.LoadTransactionDataAsync() ?? new Dictionary<string, double>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error came from {nameof(InitializeAsync)}");
                throw;
            }
        }
        /// <summary>
        /// Check if wallet has been registered
        /// </summary>
        /// <param name="wallet">Wallet's address</param>
        /// <returns>True if exists, false if it doesn't</returns>
        public bool IsWalletExists(string wallet)
        {
            try
            {
                return _walletSumsCache.ContainsKey(wallet);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error came from {nameof(IsWalletExists)}");
                throw;
            }
        }
        /// <summary>
        /// Checks if string is a real wallet, checks if string is list to output ShowList()
        /// </summary>
        /// <param name="wallet">wallet's address</param>
        /// <returns>True if real, false if not</returns>
        public bool IsWalletValid(string wallet)
        {
            try
            {
                PublicKey publicKey = new PublicKey(wallet);
                if (!publicKey.IsValid())
                {
                    Console.WriteLine("Invalid wallet address");
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch(ArgumentException)
            {
                Console.WriteLine("Invalid wallet address");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error came from {nameof(IsWalletValid)}");
                throw;
            }
        }

        /// <summary>
        /// Checks if user input was to exit the application
        /// </summary>
        /// <param name="response">User input</param>
        /// <param name="token">Token for exiting the loop</param>
        /// <returns>stops application if true, application continues if false</returns>
        public bool IsExit(string response, CancellationTokenSource token)
        {
            try
            {
                if (String.IsNullOrEmpty(response))
                {
                    token.Cancel();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("App has been stopped.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error came from {nameof(IsExit)}");
                throw;
            }
        }
    }
}
