﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto_Registerer_Transactions
{
    interface ILogicService
    {
        bool IsExit(string response, CancellationTokenSource token);
        double SumOfTransactionsByWallet(string wallet);
        bool IsWalletExists(string wallet);
        bool IsWalletValid(string wallet);
        Task InitializeAsync();
        Dictionary<string, double> GetWalletsCache();
    }
    interface IConsoleService
    {
        void SayMessage(string message, ConsoleColor color);
        void ShowList(Dictionary<string, double> _walletSumsCache);
        double GetSum();
    }
    interface IWalletIOService
    {
        void SaveTransaction(string wallet, Dictionary<string, double> _walletSumsCache, double sum);
        Task<Dictionary<string, double>> LoadTransactionDataAsync();
    }
}
