using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using ServicesContracts;

namespace Servicies
{
    public class FinnhubGetterService : IFinnhubGetterService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IFinnhubRepository _finnhubRepository;
        private readonly ILogger<FinnhubGetterService> _logger;

        public FinnhubGetterService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IFinnhubRepository finnhubRepository
            , ILogger<FinnhubGetterService> logger)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _finnhubRepository = finnhubRepository;
            _logger = logger;
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            _logger.LogInformation("{MethodName} of {ServiceName}", nameof(GetStockPriceQuote), nameof(FinnhubGetterService));

            Dictionary<string, object>? responceDictionary = await _finnhubRepository.GetStockPriceQuote(stockSymbol);

            if (responceDictionary == null)
            {
                throw new InvalidOperationException("No response from finnhub server");
            }

            if (responceDictionary.ContainsKey("error"))
            {
                throw new InvalidOperationException(Convert.ToString(responceDictionary["error"]));
            }

            return responceDictionary;
        }

        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            _logger.LogInformation("{MethodName} of {ServiceName}", nameof(GetCompanyProfile), nameof(FinnhubGetterService));

            Dictionary<string, object>? responceDictionary = await _finnhubRepository.GetCompanyProfile(stockSymbol);

            if (responceDictionary == null)
            {
                throw new InvalidOperationException("No response from finnhub server");
            }

            if (responceDictionary.ContainsKey("error"))
            {
                throw new InvalidOperationException(Convert.ToString(responceDictionary["error"]));
            }

            return responceDictionary;

        }

        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            _logger.LogInformation("{MethodName} of {ServiceName}", nameof(GetStocks), nameof(FinnhubGetterService));

            List<Dictionary<string, string>>? responceDictionaries = await _finnhubRepository.GetStocks();

            if (responceDictionaries == null)
            {
                throw new InvalidOperationException("No response from finnhub server");
            }

            foreach(Dictionary<string, string> responceDictionary in responceDictionaries)
            if (responceDictionary.ContainsKey("error"))
            {
                throw new InvalidOperationException(Convert.ToString(responceDictionary["error"]));
            }

            return responceDictionaries;
        }

    }
}
