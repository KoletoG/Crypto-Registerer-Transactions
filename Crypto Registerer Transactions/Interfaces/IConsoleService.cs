using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto_Registerer_Transactions.Interfaces
{
    public interface IConsoleService
    {
        void SayMessage(string message, ConsoleColor color);
        void ShowList(Dictionary<string, double> _walletSumsCache);
        double GetSum();
    }
}
