using Quotes.Import.Client.Models;

public interface IQuotesImportService
{
    Task ProcessAsync(QuotesImportRequest request);
}