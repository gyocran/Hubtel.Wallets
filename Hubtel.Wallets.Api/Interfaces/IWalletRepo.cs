using Hubtel.Wallets.Api.DTOs;
using Hubtel.Wallets.Api.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Collections.Generic;

namespace Hubtel.Wallets.Api.Interfaces
{
    public interface IWalletRepo
    {
        void AddWallet(Wallet wallet);
        void DeleteWallet(Wallet wallet);
        Wallet GetWalletByAccountNumber(string accountNumber);
        bool WalletAlreadyExists(string accountNumber);
        bool WalletCountExceeded(string phoneNumber);
        Wallet GetWalletById(int Id);
        List<Wallet> GetAllWallets();
        AccountScheme GetScheme(string schemeName);
        AccountType GetType(string typeName);
        List<AccountScheme> GetMomoSchemes();
        List<AccountScheme> GetCardSchemes();
        int GetWalletCountLimit();
        int GetOwnerWalletCount(string owner);
    }
}
