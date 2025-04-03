namespace Crypto_Registerer_Transactions
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Program program = new Program();
            if (!File.Exists(@"..\..\wallets.text")) 
            {
                using (StreamWriter streamWriter = new StreamWriter(@"..\..\wallets.txt", true))
                {
                    Console.WriteLine("Wallets.txt created successfully.");
                }
            }
            Console.WriteLine("Enter wallet address:");
            string wallet = Console.ReadLine();
            if (program.IsWalletExists(wallet))
            {
                Console.WriteLine($"Wallet exists with transaction sum of {program.SumOfTransactionsByWallet(wallet)}, do you want to register another transaction to this wallet? - Y / N"); 
                if (Console.ReadLine() == "Y")
                {
                    program.SaveTransaction(wallet);
                }
                else
                {
                    Console.WriteLine("Enter a wallet adress:");
                }
            }
            else
            {
                Console.WriteLine("Wallet hasn't been registered, do you want to register a transaction? - Y / N");
                if (Console.ReadLine() == "Y")
                {
                    program.SaveTransaction(wallet);
                }
                else
                {
                    Console.WriteLine("Enter a wallet address:");
                }
            }
        }
        public void SaveTransaction(string wallet)
        {
            Console.WriteLine("Enter sum to register:");
            int sum = int.Parse(Console.ReadLine());
            using (StreamWriter strW = new StreamWriter(@"..\..\wallets.txt", true))
            {
                strW.WriteLine(wallet);
                strW.WriteLine(sum);
            }
        }
        public int SumOfTransactionsByWallet(string wallet)
        {
            int wholeSum = 0;
            using (StreamReader streamReader = new StreamReader(@"..\..\wallets.txt"))
            {
                while (!streamReader.EndOfStream)
                {
                    if (streamReader.ReadLine() == wallet)
                    {
                        wholeSum += int.Parse(streamReader.ReadLine());
                    }
                }
            }
            return wholeSum;
        }
        public bool IsWalletExists(string wallet)
        {
            try
            {
                return File.ReadLines(@"..\..\wallets.txt").Contains(wallet);
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex, "Error in {MethodName}", nameof(IsWalletExists));
                throw new Exception(ex.ToString());
            }
        }
    }
}
