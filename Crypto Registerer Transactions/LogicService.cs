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
        private HashSet<string> _walletsCache = new();
        public LogicService(ILogger<LogicService> logger)
        {
            _logger = logger;
        }
        public void LoadWallets()
        {
            if (!File.Exists(@"..\..\wallets.txt")) return;

            _walletsCache = File.ReadLines(@"..\..\wallets.txt")
                                .Where((_, index) => index % 2 == 0)
                                .ToHashSet();
        }
        public void IsExit(string response, CancellationTokenSource token)
        {
            try
            {
                if (String.IsNullOrEmpty(response))
                {
                    token.Cancel();
                    throw new OperationCanceledException();
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("App has been stopped.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error came from {nameof(IsExit)}");
                throw new Exception(ex.ToString());
            }
        }
        public void SaveTransaction(string wallet)
        {
            try
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
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Transaction saved successfully!");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                else
                {
                    Console.WriteLine("Invalid number.");
                    SaveTransaction(wallet);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error came from {nameof(SaveTransaction)}");
                throw new Exception(ex.ToString());
            }
        }
        public double SumOfTransactionsByWallet(string wallet)
        {
            try
            {
                double wholeSum = 0;
                using (StreamReader streamReader = new StreamReader(@"..\..\wallets.txt"))
                {
                    while (!streamReader.EndOfStream)
                    {
                        if (streamReader.ReadLine() == wallet)
                        {
                            wholeSum += double.Parse(streamReader.ReadLine() ?? "0");
                        }
                    }
                }
                return wholeSum;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error came from {nameof(SumOfTransactionsByWallet)}");
                throw new Exception(ex.ToString());
            }
        }
        public bool IsWalletExists(string wallet)
        {
            try
            {
                return _walletsCache.Contains(wallet);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error came from {nameof(IsWalletExists)}");
                throw new Exception(ex.ToString());
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
