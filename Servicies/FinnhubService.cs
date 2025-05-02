using System.Text.Json;
using Microsoft.Extensions.Configuration;
using ServicesContracts;

namespace Servicies
{
    public class FinnhubService : IFinnhubService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public FinnhubService(IHttpClientFactory httpClientFactory, IConfiguration configuration) 
        { 
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={_configuration["FinnhubToken"]} "),
                    Method = HttpMethod.Get,

                };
                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                Stream stream = httpResponseMessage.Content.ReadAsStream();

                StreamReader reader = new StreamReader(stream);

                string response = reader.ReadToEnd();

                Dictionary<string, object>? responceDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(response);

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

        public async Task<Dictionary<string, object>?>  GetCompanyProfile(string stockSymbol)
        {
            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={_configuration["FinnhubToken"]} "),
                    Method = HttpMethod.Get,

                };
                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                Stream stream = httpResponseMessage.Content.ReadAsStream();

                StreamReader reader = new StreamReader(stream);

                string response = reader.ReadToEnd();

                Dictionary<string, object>? responceDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(response);

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
}
