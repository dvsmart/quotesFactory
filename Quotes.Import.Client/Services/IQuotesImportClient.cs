namespace Quotes.Import.Client.Services
{
    public interface IQuotesImportClient
    {
        /// <summary>
        /// Get quotes grouping details
        /// </summary>
        /// <returns></returns>
        Task<HttpResponseMessage> GetQuoteGroupsAsync();
    }
}
