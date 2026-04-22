using Newtonsoft.Json;

namespace GLMS.Services
{
    public class CurrencyService
    {
        private readonly HttpClient _client;

        public CurrencyService(HttpClient client)
        {
            _client = client;
        }

        public async Task<decimal> ConvertUsdToZar(decimal usd)
        {
            var response = await _client.GetStringAsync("https://api.exchangerate-api.com/v4/latest/USD");
            dynamic data = JsonConvert.DeserializeObject(response);
            decimal rate = data.rates.ZAR;

            return usd * rate;
        }
    }
}
