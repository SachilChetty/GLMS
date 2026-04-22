namespace GLMS.Patterns
{
    public class CurrencyService
    {
        private static CurrencyService _instance;

        private CurrencyService() { }

        public static CurrencyService GetInstance()
        {
            if (_instance == null)
                _instance = new CurrencyService();

            return _instance;
        }
    }
}
