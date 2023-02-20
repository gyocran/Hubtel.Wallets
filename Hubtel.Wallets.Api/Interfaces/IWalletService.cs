using Hubtel.Wallets.Api.DTOs;
using Hubtel.Wallets.Api.Models;
using System.Collections.Generic;

namespace Hubtel.Wallets.Api.Interfaces
{
    public interface IWalletService
    {
        bool DeleteWallet(int Id);
        WalletDto GetWalletById(int Id);
        List<WalletDto> GetAllWallets(AllWalletsParameters queryParams);
        int AddWallet(WalletDto wallet);
        bool WalletAlreadyExists(WalletDto wallet);
        bool WalletCountExceeded(WalletDto wallet);
        AccountScheme GetScheme(WalletDto wallet);
        AccountType GetType(WalletDto wallet);
        bool SchemeDoesNotExist(WalletDto wallet);
        bool TypeDoesNotExist(WalletDto wallet);
        bool AccountNumberLengthIsInvalid(WalletDto wallet);
        bool AccountNumberContainsNonNumeric(WalletDto wallet);
        bool OwnerContainsNonNumeric(WalletDto wallet);
        bool OwnerLengthIsInvalid(WalletDto wallet);
        string ReturnWalletError(WalletDto wallet);
        bool AccountIsCard(WalletDto wallet);
        bool TypeSchemeMismatch(WalletDto wallet);
        Wallet ConvertDtoToWalletEntity(WalletDto walletDto);
    }
}
