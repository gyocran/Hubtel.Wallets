namespace Hubtel.Wallets.Api.Helpers
{
    public class Utilities
    {
        public static string TrimCardNumber(string cardNumber)
        {
            return cardNumber.Substring(0, 6);
        }
    }
}
