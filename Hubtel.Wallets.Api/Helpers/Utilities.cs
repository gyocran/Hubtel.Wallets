using Hubtel.Wallets.Api.DTOs;
using System.Text.RegularExpressions;

namespace Hubtel.Wallets.Api.Helpers
{
    public class Utilities
    {
        public static void TrimCardNumber(WalletDto wallet)
        {
            wallet.AccountNumber = wallet.AccountNumber.Substring(0, 6);
        }
        public static void RemoveWhiteSpaces(WalletDto wallet)
        {
            wallet.AccountNumber = Regex.Replace(wallet.AccountNumber, @"\s+", "");
        }

        public static bool ContainsNonNumericCharacters(string data)
        {
            Regex regex = new Regex(@"^[0-9]+$");
            return !regex.IsMatch(data);
        }
    }
}
