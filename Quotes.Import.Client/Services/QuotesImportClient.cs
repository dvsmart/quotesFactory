using Microsoft.Extensions.Configuration;

namespace Quotes.Import.Client.Services
{
    public class QuotesImportClient : IQuotesImportClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _quotesApiBaseUrl;
        private readonly string _quotesApiKey;

        public QuotesImportClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _quotesApiBaseUrl = configuration.GetValue<string>("Quotes.Import.Api.BaseUrl");
            _quotesApiKey = configuration.GetValue<string>("Quotes.Import.Api.Key");
        }

        /// <inheritdoc />
        public async Task<HttpResponseMessage> GetQuoteGroupsAsync()
        {
            var quotesGrouppingUri = "/QuoteGroups";
            _httpClient.BaseAddress = new Uri(_quotesApiBaseUrl);
            _httpClient.DefaultRequestHeaders.Add("X-Api-Key", _quotesApiKey);
            return await _httpClient.GetAsync(quotesGrouppingUri);
        }
    }
}
