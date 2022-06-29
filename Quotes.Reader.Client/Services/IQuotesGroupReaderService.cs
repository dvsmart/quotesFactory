using Quotes.Reader.Client.Models;

namespace Quotes.Reader.Client.Services
{
    public interface IQuotesGroupReaderService
    {
        Task ReadGroupQuotesAsync(QuotesGroupReaderRequest groupReaderRequest);
    }
}
