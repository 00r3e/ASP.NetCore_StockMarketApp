namespace ServicesContracts
{
    public interface IFinnhubGetterService
    {
        /// <summary>
        /// Retrieves company Details from finnhub api
        /// </summary>
        /// <param name="stockSymbol">The symbol of the name of the company</param>
        /// <returns>Dictionary with the details of the company</returns>
        Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol);
        /// <summary>
        /// Retrieves Details for the Stock price of a company, from finnhub api
        /// </summary>
        /// <param name="stockSymbol">The symbol of the name of the company</param>
        /// <returns>Dictionary with the details of the company price</returns>
        Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<List<Dictionary<string, string>>?> GetStocks();
    }
}
