using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Crypto_Registerer_Transactions
{
    internal class Program
    {

        private static CancellationTokenSource token = new CancellationTokenSource();
        private ILogicService _logicService;
        private IServiceProvider _serviceProvider;
        private ILogger<Program> _logger;
        public Program()
        {
            Log.Logger = new LoggerConfiguration().WriteTo.File(@"log.txt").CreateLogger();
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<ILogicService, LogicService>();
            serviceCollection.AddLogging(x => x.AddSerilog());
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }
        static void Main(string[] args)
        {
            Program program = new Program();
            program.Run();
        }
        private void Run()
        {
            _logicService = _serviceProvider.GetRequiredService<ILogicService>();
            _logger = _serviceProvider.GetRequiredService<ILogger<Program>>();
            if (!File.Exists(@"..\..\wallets.text"))
            {
                using (StreamWriter streamWriter = new StreamWriter(@"..\..\wallets.txt", true))
                {
                    Console.WriteLine("Wallets.txt created successfully.");
                }
            }
            Console.WriteLine("If you want to stop the application, either just press ENTER or just exit from the X");
            while (!token.IsCancellationRequested)
            {
                Console.WriteLine("Enter wallet address:");
                string wallet = Console.ReadLine() ?? string.Empty;
                _logicService.IsExit(wallet, token);
                if (_logicService.IsWalletExists(wallet))
                {
                    Console.WriteLine($"Wallet exists with transaction sum of {_logicService.SumOfTransactionsByWallet(wallet)}, do you want to register another transaction to this wallet? - Y / N");
                    string response = Console.ReadLine() ?? string.Empty;
                    _logicService.IsExit(response, token);
                    if (_logicService.IsResponseY(response))
                    {
                        _logicService.SaveTransaction(wallet);
                    }
                    else
                    {
                        Console.WriteLine("Transaction registration declined.");
                    }
                }
                else
                {
                    Console.WriteLine("Wallet hasn't been registered, do you want to register a transaction? - Y / N");
                    string response = Console.ReadLine() ?? string.Empty;
                    _logicService.IsExit(response, token);
                    if (_logicService.IsResponseY(response))
                    {
                        _logicService.SaveTransaction(wallet);
                    }
                    else
                    {
                        Console.WriteLine("Transaction registration declined.");
                    }
                }
            }
            Console.WriteLine("App has stopped");
        }
        
    }
}
