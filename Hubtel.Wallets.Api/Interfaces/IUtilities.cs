using Hubtel.Wallets.Api.DTOs;

namespace Hubtel.Wallets.Api.Interfaces
{
    public interface IUtilities
    {
        void TrimCardNumber(WalletDto wallet);
        void RemoveWhiteSpaces(string number);
        bool ContainsNonNumericCharacters(string data);
    }
}
