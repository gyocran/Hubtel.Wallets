using Hubtel.Wallets.Api.DTOs;
using Hubtel.Wallets.Api.Models;
using System.Collections.Generic;

namespace Hubtel.Wallets.Api.Interfaces
{
    public interface IWalletService
    {
        bool DeleteWallet(int Id);
        WalletDto GetWalletById(int Id);
        List<WalletDto> GetAllWallets();
        string AddWallet(WalletDto wallet);
    }
}
