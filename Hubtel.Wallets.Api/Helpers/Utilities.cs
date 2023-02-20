using Hubtel.Wallets.Api.DTOs;
using Hubtel.Wallets.Api.Interfaces;
using System.Text.RegularExpressions;

namespace Hubtel.Wallets.Api.Helpers
{
    public class Utilities : IUtilities
    {
        public void TrimCardNumber(WalletDto wallet)
        {
            wallet.AccountNumber = wallet.AccountNumber.Substring(0, 6);
        }
        public void RemoveWhiteSpaces(string number)
        {
            number = Regex.Replace(number, @"\s+", "");
        }

        public bool ContainsNonNumericCharacters(string data)
        {
            //Regex regex = new Regex(@"^[0-9]+$");
            Regex regex = new Regex(@"^[0-9_]+$");
            return !regex.IsMatch(data);
        }
    }
}
