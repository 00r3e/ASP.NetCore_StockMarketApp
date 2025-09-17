namespace ServicesContracts
{
    public interface IFinnhubSearcherService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stockSymbolToSearch"></param>
        /// <returns></returns>
        Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch);
    }
}
