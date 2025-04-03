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
        public void LoadTransactionData()
        {
            try
            {
                _walletSumsCache.Clear();
                var lines = File.ReadAllLines(@"..\..\wallets.txt");
                for (int i = 0; i < lines.Length - 1; i += 2)
                {
                    string wallet = lines[i];
                    if (double.TryParse(lines[i + 1], out double sum))
                    {
                        if (_walletSumsCache.ContainsKey(wallet))
                            _walletSumsCache[wallet] += sum;
                        else
                            _walletSumsCache[wallet] = sum;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error came from {nameof(LoadTransactionData)}");
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
                            _walletSumsCache.Add(wallet,sum);
                        }
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Transaction saved successfully!");
                        Console.ForegroundColor = ConsoleColor.Gray;
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
            PublicKey publicKey = new PublicKey(wallet);
            return publicKey.IsValid();
        }
        public void SayTransactionDeclined()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Transaction registration declined.");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
