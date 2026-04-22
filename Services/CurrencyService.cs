namespace GLMS.Services
{
    public class CurrencyService
    {
        private readonly HttpClient _httpClient;

        // The constructor MUST take HttpClient as a parameter
        public CurrencyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> ConvertUsdToZar(decimal usdAmount)
        {
            
            decimal exchangeRate = 18.50m;
            return usdAmount * exchangeRate;
        }
    }
}