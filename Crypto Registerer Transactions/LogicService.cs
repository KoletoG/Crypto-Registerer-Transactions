using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Solnet.Wallet;
namespace Crypto_Registerer_Transactions
{
    internal class LogicService : ILogicService
    {
        private ILogger<LogicService> _logger;
        private Dictionary<string, double> _walletSumsCache = new();
        public LogicService(ILogger<LogicService> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// Fills Dictionary<> with wallets
        /// </summary>
        /// <returns>Filled _walletSumsCache</returns>
        public async Task LoadTransactionDataAsync()
        {
            try
            {
                _walletSumsCache.Clear();
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
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error came from {nameof(LoadTransactionDataAsync)}");
                throw;
            }
        }
        /// <summary>
        /// Writes wallet with sum to wallets.txt
        /// </summary>
        /// <param name="wallet">Wallet address</param>
        public void SaveTransaction(string wallet)
        {
            try
            {
                while (true)
                {
                    Console.WriteLine("Enter sum to register:");
                    string response = Console.ReadLine() ?? string.Empty;
                    if (double.TryParse(response, out double result))
                    {
                        double sum = result;
                        using (StreamWriter strW = new StreamWriter(@"wallets.txt", true))
                        {
                            strW.WriteLine(wallet);
                            strW.WriteLine(sum);
                        }
                        if (_walletSumsCache.ContainsKey(wallet))
                        {
                            _walletSumsCache[wallet] += sum;
                        }
                        else
                        {
                            _walletSumsCache.Add(wallet, sum);
                        }
                        SayMessage("Transaction saved successfully!", ConsoleColor.Green);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid number.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error came from {nameof(SaveTransaction)}");
                throw;
            }
        }
        /// <summary>
        /// Writes a message with the specified color
        /// </summary>
        /// <param name="message">Message to output</param>
        /// <param name="color">Color of the message</param>
        public void SayMessage(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Gray;
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
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error came from {nameof(SumOfTransactionsByWallet)}");
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
                if (wallet.ToLower() == "list")
                {
                    ShowList();
                    return false;
                }
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
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error came from {nameof(IsWalletValid)}");
                throw;
            }
        }
        /// <summary>
        /// Shows list of all wallets and their sums that have been registered
        /// </summary>
        private void ShowList()
        {
            try
            {
                foreach (var key in _walletSumsCache.Keys)
                {
                    Console.WriteLine($"{key} with sum of: {_walletSumsCache[key]}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error came from {nameof(ShowList)}");
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
                    throw new OperationCanceledException();
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
