using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using ServicesContracts;

namespace Servicies
{
    public class FinnhubSearcherService : IFinnhubSearcherService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IFinnhubRepository _finnhubRepository;
        private readonly ILogger<FinnhubSearcherService> _logger;

        public FinnhubSearcherService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IFinnhubRepository finnhubRepository
            , ILogger<FinnhubSearcherService> logger)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _finnhubRepository = finnhubRepository;
            _logger = logger;
        }

        public async Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
        {
            _logger.LogInformation("{MethodName} of {ServiceName}", nameof(SearchStocks), nameof(FinnhubSearcherService));

            Dictionary<string, object>? responceDictionary = await _finnhubRepository.SearchStocks(stockSymbolToSearch);

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
    }
}
