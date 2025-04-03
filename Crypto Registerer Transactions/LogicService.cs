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
        public LogicService(ILogger<LogicService> logger)
        {
            _logger = logger;
        }
        public bool IsResponseY(string response)
        {
            try
            {
                return response.ToUpper() == "Y" || response.ToUpper() == "YES";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error came from {nameof(IsResponseY)}");
                throw new Exception(ex.ToString());
            }
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
                if (double.TryParse(response, out double result)) {
                    double sum = result; 
                    using (StreamWriter strW = new StreamWriter(@"..\..\wallets.txt", true))
                    {
                        strW.WriteLine(wallet);
                        strW.WriteLine(sum);
                    }
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
                return File.ReadLines(@"..\..\wallets.txt").Contains(wallet);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error came from {nameof(IsResponseY)}");
                throw new Exception(ex.ToString());
            }
        }
        public bool IsWalletValid(string wallet)
        {
            PublicKey publicKey = new PublicKey(wallet);
            return publicKey.IsValid();
        }
    }
}
