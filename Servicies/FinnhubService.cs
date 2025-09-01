using System.Text.Json;
using Microsoft.Extensions.Configuration;
using RepositoryContracts;
using ServicesContracts;

namespace Servicies
{
    public class FinnhubService : IFinnhubService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IFinnhubRepository _finnhubRepository;

        public FinnhubService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IFinnhubRepository finnhubRepository)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _finnhubRepository = finnhubRepository;
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
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

        public async Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
        {
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
