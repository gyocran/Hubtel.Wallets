using Hubtel.Wallets.Api.DTOs;
using Hubtel.Wallets.Api.Models;
using System.Collections.Generic;

namespace Hubtel.Wallets.Api.Interfaces
{
    public interface IWalletRepo
    {
        void AddWallet(Wallet wallet);
        void DeleteWallet(Wallet wallet);
        bool WalletAlreadyExists(string accountNumber);
        bool WalletCountExceeded(string phoneNumber);
        Wallet GetWalletById(int Id);
        List<Wallet> GetAllWallets();
        AccountScheme GetScheme(string schemeName);
        AccountType GetType(string typeName);
        int WalletCountLimit();
    }
}
