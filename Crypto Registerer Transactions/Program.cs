using System.Transactions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
namespace Crypto_Registerer_Transactions
{
    internal class Program
    {

        private CancellationTokenSource token = new CancellationTokenSource();
        private ILogicService _logicService;
        private IServiceProvider _serviceProvider;
        private ILogger<Program> _logger;
        public Program()
        {
            _serviceProvider = BuildService();
            _logicService = _serviceProvider.GetRequiredService<ILogicService>();
            _logger = _serviceProvider.GetRequiredService<ILogger<Program>>();
        }
        private IServiceProvider BuildService()
        {
            Log.Logger = new LoggerConfiguration().WriteTo.File(@"log.txt").CreateLogger();
            return new ServiceCollection()
                .AddSingleton<ILogicService, LogicService>()
                .AddLogging(x => x.AddSerilog())
                .BuildServiceProvider();
        }
        static void Main(string[] args)
        {
            Program program = new Program();
            program.Run();
        }
        private void Run()
        {
            try
            {
                _logicService.LoadWallets();
                Console.WriteLine("If you want to stop the application, either just press ENTER or just exit from the X");
                while (!token.IsCancellationRequested)
                {
                    Console.WriteLine("Enter wallet address:");
                    string wallet = Console.ReadLine() ?? string.Empty;
                    _logicService.IsExit(wallet, token);
                    if (_logicService.IsWalletValid(wallet))
                    {
                        if (_logicService.IsWalletExists(wallet))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Wallet exists with transaction sum of {_logicService.SumOfTransactionsByWallet(wallet)}, do you want to register another transaction to this wallet? - Y / N");
                            Console.ForegroundColor = ConsoleColor.Gray;
                            string response = Console.ReadLine() ?? string.Empty;
                            _logicService.IsExit(response, token);
                            switch (response.ToUpper())
                            {
                                case "Y": _logicService.SaveTransaction(wallet);break;
                                case "N":_logicService.SayTransactionDeclined();break;
                                default: Console.WriteLine("Invalid response.");break;
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Wallet hasn't been registered, do you want to register a transaction? - Y / N");
                            Console.ForegroundColor = ConsoleColor.Gray;
                            string response = Console.ReadLine() ?? string.Empty;
                            _logicService.IsExit(response, token);
                            switch (response.ToUpper())
                            {
                                case "Y": _logicService.SaveTransaction(wallet); break;
                                case "N": _logicService.SayTransactionDeclined(); break;
                                default: Console.WriteLine("Invalid response."); break;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid wallet address");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error came from {nameof(Run)}");
                throw;
            }
        }

    }
}
