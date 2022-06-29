
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quotes.Import.Client;
using Quotes.Import.Client.Extensions;
using Quotes.Import.Client.Models;
using Quotes.Import.Client.ServiceModels;
using Quotes.Import.Client.Services;

public class QuotesImportService : IQuotesImportService
{
    private readonly ILogger<QuotesImportService> _logger;
    private readonly int _maxDegreeOfParallelism;
    private readonly IQuotesImportClient _httpClientService;
    private readonly IValidator<QuotesImportRequest> _quotesImportRequestValidator;
    private IEnumerable<ProductQuoteGroup> _quoteGroups;

    public QuotesImportService(ILogger<QuotesImportService> logger,
        IConfiguration configuration,
        IQuotesImportClient httpClientService,
        IValidator<QuotesImportRequest> quotesImportRequestValidator)
    {
        _logger = logger;
        _maxDegreeOfParallelism = Convert.ToInt32(configuration["MaxDegreeOfParallelism"]);
        _httpClientService = httpClientService;
        _quotesImportRequestValidator = quotesImportRequestValidator;
    }

    public async Task ProcessAsync(QuotesImportRequest request)
    {
        try
        {
            // Check if the request object is valid.
            var validationResult = await _quotesImportRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                _logger.LogError(string.Join(",", validationResult.Errors));
                return;
            }

            // Get All quotes group details.
            _quoteGroups = await GetQuoteGroupsAsync();

            var parallismOptions = new ParallelOptions()
            {
                MaxDegreeOfParallelism = _maxDegreeOfParallelism
            };
            Parallel.ForEach(Directory.EnumerateFiles(request.InputDirectory, "*.csv"), 
                parallismOptions, async file =>
            {
                await ProcessFileAsync(file, request.StorageDirectory);
            });

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Unable to process the quotes from the inputdirectory: {request.InputDirectory} into the storageDirectory {request.StorageDirectory}");
        }
    }

    private async Task ProcessFileAsync(string file, string storageDirectory)
    {
        var quoteDetails = QuotesFileReaderHelper.ConstructQuoteDetails(file);
        GroupQuotesAndSaveAsJson(storageDirectory, quoteDetails);
    }

    private void GroupQuotesAndSaveAsJson(string storageDirectory, IEnumerable<ProductQuote> quoteDetails)
    {

        if (quoteDetails != null && quoteDetails.Any())
        {
            foreach (var item in quoteDetails)
            {
                var groupDetail = _quoteGroups?.FirstOrDefault(x => string.Equals(x.ProductId, item.ProductId, StringComparison.OrdinalIgnoreCase));
                if (groupDetail != null)
                {
                    item.GroupName = groupDetail.GroupName;
                }
            }

            quoteDetails = quoteDetails.Where(x => !string.IsNullOrEmpty(x.GroupName)).ToList();
        }

        var quotesGroup = quoteDetails?.GroupBy(x => x.GroupName);
        QuotesFileWriterHelper.WriteQuotesIntoFile(quotesGroup, quoteDetails, storageDirectory);

        return;
    }


    private async Task<IEnumerable<ProductQuoteGroup>> GetQuoteGroupsAsync()
    {
        var quotesGroupHttpResponse = await _httpClientService.GetQuoteGroupsAsync();
        quotesGroupHttpResponse.EnsureSuccessStatusCode();
        var quotesGroupDetails = JsonConvert.DeserializeObject<IEnumerable<ProductQuoteGroup>>(
                await quotesGroupHttpResponse.Content.ReadAsStringAsync());

        if (quotesGroupDetails == null || !quotesGroupDetails.Any())
        {
            _logger.LogError("Quotes Group Mapping detail is not available.");
            return null;
        }
        return quotesGroupDetails;
    }
}