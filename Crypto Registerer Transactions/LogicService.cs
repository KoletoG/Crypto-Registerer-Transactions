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
        public async Task LoadTransactionDataAsync()
        {
            try
            {
                _walletSumsCache.Clear();
                var lines = await File.ReadAllLinesAsync(@"..\..\wallets.txt");
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
                        using (StreamWriter strW = new StreamWriter(@"..\..\wallets.txt", true))
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
        public void SayMessage(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
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
