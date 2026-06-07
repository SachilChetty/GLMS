using System.Text.Json;

namespace GLMS.Services
{
    public class CurrencyService
    {
        private readonly HttpClient _httpClient;
        // This is a free API key from ExchangeRate-API (You can get your own at exchangerate-api.com)
        private const string ApiUrl = "https://v6.exchangerate-api.com/v6/YOUR-API-KEY/pair/USD/ZAR";

        public CurrencyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> ConvertUsdToZar(decimal usdAmount)
        {
            try
            {
                // 1. Call the API
                var response = await _httpClient.GetAsync(ApiUrl);
                response.EnsureSuccessStatusCode();

                // 2. Read the JSON content
                var jsonString = await response.Content.ReadAsStringAsync();

                // 3. Parse the specific conversion_rate field
                using (JsonDocument doc = JsonDocument.Parse(jsonString))
                {
                    decimal exchangeRate = doc.RootElement
                        .GetProperty("conversion_rate")
                        .GetDecimal();

                    return usdAmount * exchangeRate;
                }
            }
            catch (Exception)
            {
                // Fallback rate in case the internet is down or API fails
                decimal fallbackRate = 18.50m;
                return usdAmount * fallbackRate;
            }
        }
    }
}