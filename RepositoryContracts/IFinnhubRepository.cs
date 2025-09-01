using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryContracts
{
    public interface IFinnhubRepository
    {
        /// <summary>
        /// Retrieves company details from finnhub api
        /// </summary>
        /// <param name="stockSymbol">The symbol of the name of the company</param>
        /// <returns>Dictionary with the details of the company</returns>
        Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol);

        /// <summary>
        /// Retrieves details for the Stock price of a company, from finnhub api
        /// </summary>
        /// <param name="stockSymbol">The symbol of the name of the company</param>
        /// <returns>Dictionary with the details of the company price</returns>
        Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol);

        /// <summary>
        /// Retrieves the details of all Stocks
        /// </summary>
        /// <returns>List of Dictionary with the details of the Stocks</returns>
        Task<List<Dictionary<string, string>>?> GetStocks();

        /// <summary>
        /// Retrieves the details of Stocks that matching with the stock symbol to search
        /// </summary>
        /// <param name="stockSymbolToSearch">The symbol of the name of the company to search</param>
        /// <returns></returns>
        Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch);
    }
}
