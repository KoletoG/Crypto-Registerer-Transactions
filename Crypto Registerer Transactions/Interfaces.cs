﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto_Registerer_Transactions
{
    interface ILogicService
    {
        void IsExit(string response, CancellationTokenSource token);
        void SaveTransaction(string wallet);
        double SumOfTransactionsByWallet(string wallet);
        bool IsWalletExists(string wallet);
        bool IsWalletValid(string wallet);
        void SayTransactionDeclined();
        void LoadWallets();
    }
}
