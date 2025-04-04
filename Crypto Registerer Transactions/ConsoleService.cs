using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Crypto_Registerer_Transactions
{
    internal class ConsoleService : IConsoleService
    {
        private ILogger<ConsoleService> _logger;
        public ConsoleService(ILogger<ConsoleService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Writes a message with the specified color
        /// </summary>
        /// <param name="message">Message to output</param>
        /// <param name="color">Color of the message</param>
        public void SayMessage(string message, ConsoleColor color)
        {
            try
            {
                Console.ForegroundColor = color;
                Console.WriteLine(message);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error came from {nameof(SayMessage)}");
                throw;
            }
        }
        /// <summary>
        /// Shows list of all wallets and their sums that have been registered
        /// </summary>
        public void ShowList(Dictionary<string, double> _walletSumsCache)
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
        public double GetSum()
        {
            try
            {
                while (true)
                {
                    Console.WriteLine("Enter sum to register:");
                    string sum = Console.ReadLine() ?? string.Empty;
                    if (double.TryParse(sum, out double result))
                    {
                        return result;
                    }
                    else
                    {
                        Console.WriteLine("Invalid number.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error came from {nameof(GetSum)}");
                throw;
            }
        }
    }
}
