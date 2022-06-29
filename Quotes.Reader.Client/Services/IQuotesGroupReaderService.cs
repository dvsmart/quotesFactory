using Quotes.Reader.Client.Models;

namespace Quotes.Reader.Client.Services
{
    public interface IQuotesGroupReaderService
    {
        Task ProcessAsync(QuotesGroupReaderRequest groupReaderRequest);
    }
}
