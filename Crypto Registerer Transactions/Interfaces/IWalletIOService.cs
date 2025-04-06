using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto_Registerer_Transactions.Interfaces
{
    public interface IWalletIOService
    {
        void SaveTransaction(string wallet, Dictionary<string, double> _walletSumsCache, double sum);
        Task<Dictionary<string, double>> LoadTransactionDataAsync();
    }
}
