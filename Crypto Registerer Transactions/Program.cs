using System.Threading.Tasks;
using System.Transactions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Solnet.Wallet;
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
        /// <summary>
        /// Builds Dependency injection
        /// </summary>
        /// <returns>DI Service</returns>
        private IServiceProvider BuildService()
        {
            Log.Logger = new LoggerConfiguration().WriteTo.File(@"log.txt").CreateLogger();
            return new ServiceCollection()
                .AddSingleton<ILogicService, LogicService>()
                .AddLogging(x => x.AddSerilog())
                .BuildServiceProvider();
        }
        /// <summary>
        /// CPU of the program
        /// </summary>
        /// <param name="args"></param>
        /// <returns>Program running</returns>
        static async Task Main(string[] args)
        {
            if (!File.Exists(@"wallets.txt"))
            {
                using (StreamWriter str = new StreamWriter(@"wallets.txt"))
                {

                }
            }
            Program program = new Program();
            await program.Run();
        }
        /// <summary>
        /// Main logic of the application
        /// </summary>
        /// <returns></returns>
        private async Task Run()
        {
            try
            {
                await _logicService.LoadTransactionDataAsync();
                _logicService.SayMessage("If you want to stop the application, either just press ENTER or just exit from the X", ConsoleColor.Blue);
                while (!token.IsCancellationRequested)
                {
                    Console.WriteLine("Enter wallet address or type list for all wallets and sums:"); 
                    string wallet = Console.ReadLine() ?? string.Empty;
                    if (!_logicService.IsExit(wallet, token))
                    {
                        if (_logicService.IsWalletValid(wallet))
                        {
                            if (_logicService.IsWalletExists(wallet))
                            {
                                _logicService.SayMessage($"Wallet exists with transaction sum of {_logicService.SumOfTransactionsByWallet(wallet)}, do you want to register another transaction to this wallet? - Y / N", ConsoleColor.Red);
                                string response = Console.ReadLine() ?? string.Empty;
                                _logicService.IsExit(response, token);
                                switch (response.ToUpper())
                                {
                                    case "Y": _logicService.SaveTransaction(wallet); break;
                                    case "N": _logicService.SayMessage("Transaction registration declined.", ConsoleColor.Red); break;
                                    default: Console.WriteLine("Invalid response."); break;
                                }
                            }
                            else
                            {
                                _logicService.SayMessage("Wallet hasn't been registered, do you want to register a transaction? - Y / N", ConsoleColor.Green);
                                string response = Console.ReadLine() ?? string.Empty;
                                _logicService.IsExit(response, token);
                                switch (response.ToUpper())
                                {
                                    case "Y": _logicService.SaveTransaction(wallet); break;
                                    case "N": _logicService.SayMessage("Transaction registration declined.",ConsoleColor.Red); break;
                                    default: Console.WriteLine("Invalid response."); break;
                                }
                            }
                        }
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
