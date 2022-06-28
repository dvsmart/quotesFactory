
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quotes.Import.Client; 
using System.Threading.Tasks;

public class QuotesImportService : IQuotesImportService
{
    private readonly ILogger<QuotesImportService> _logger;
    private readonly int  _maxDegreeOfParallelism;
    private readonly IQuotesGroupMapperService _quotesGroupMapper;
    public QuotesImportService(ILogger<QuotesImportService> logger, IConfiguration configuration,
        IQuotesGroupMapperService quotesGroupMapper)
    {
        _logger = logger;
        _maxDegreeOfParallelism = Convert.ToInt32( configuration["MaxDegreeOfParallelism"]);
        _quotesGroupMapper = quotesGroupMapper;
    }

    public void ProcessQuotes(string inputDirectory, string storageDirectory)
    {
        try
        {
            if (string.IsNullOrEmpty(inputDirectory) && !Directory.Exists(@$"{inputDirectory}"))
            {
                _logger.LogError("Invalid input directory");
                return;
            }

            if (string.IsNullOrEmpty(storageDirectory) && !Directory.Exists(@$"{storageDirectory}"))
            {
                _logger.LogError("Invalid storage directory");
                return;
            }

            var parallismOptions = new ParallelOptions()
            {
                MaxDegreeOfParallelism = _maxDegreeOfParallelism
            }; 
            Parallel.ForEach(Directory.EnumerateFiles(inputDirectory, "*.csv"), parallismOptions, file =>
            {
                ProcessFile(file, storageDirectory);
            });

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Unable to process the quotes from the inputdirectory: {inputDirectory} into the storageDirectory {storageDirectory}");
        }
    }

    private void ProcessFile(string file, string storageDirectory)
    {
        var sr = new StreamReader(file);
        var quoteDetails = new List<QuoteDetail>();
        string? line;
        var header = true;
        while ((line = sr.ReadLine()) != null)
        {
            var row = line.Split(',');
            if (header)
            {
                header = false;
                continue;
            }
            else 
            { 
                quoteDetails.Add(new QuoteDetail
                {
                    ProductId = row[0],
                    AsOfDate = DateTime.Parse(row[1]),
                    Value = string.IsNullOrWhiteSpace(row[2]) ? 0 : Convert.ToDecimal(row[2])
                });
            }
            
        }

        GroupQuotesAndSaveFile(storageDirectory, quoteDetails);
    }

    private void GroupQuotesAndSaveFile(string storageDirectory,List<QuoteDetail> quoteDetails)
    {
        var quotesGroupDetails = _quotesGroupMapper.GetQuotesGroupDetails();

        if (quotesGroupDetails == null || !quotesGroupDetails.Any())
        {
            _logger.LogError("Quotes Group Mapping detail is not available.");
        }

        if (quoteDetails != null && quoteDetails.Any())
        {
            foreach (var item in quoteDetails)
            {
                var groupDetail = quotesGroupDetails?.FirstOrDefault(x => string.Equals(x.ProductId, item.ProductId, StringComparison.OrdinalIgnoreCase));
                if (groupDetail != null)
                {
                    item.GroupName = groupDetail.GroupName;
                }
            }

            quoteDetails = quoteDetails.Where(x => !string.IsNullOrEmpty(x.GroupName)).ToList();
        }

        var quotesGroup = quoteDetails?.GroupBy(x => x.GroupName);
        foreach (var item in quotesGroup)
        {
            var path = storageDirectory + @$"\{item.Key}.json";
            if (File.Exists(path))
            {
                var jsonData = File.ReadAllText(path);
                // De-serialize to object or create new list
                var existingQuoteDetails = JsonConvert.DeserializeObject<List<QuoteDetail>>(jsonData)
                                      ?? new List<QuoteDetail>();

                // Add any new employees
                existingQuoteDetails.AddRange(quoteDetails);
                

                // Update json data string
                jsonData = JsonConvert.SerializeObject(GetUniqueProductIdsWithMaxValueQuotes(existingQuoteDetails));
                File.WriteAllText(path, jsonData);
            }
            else
            {
                var quotesToBeAdded = GetUniqueProductIdsWithMaxValueQuotes(item.ToList());
                using (StreamWriter file = File.CreateText(path))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    //serialize object directly into file stream
                    serializer.Serialize(file, quotesToBeAdded);
                }
            }
        } 
        return;
    }

    private static List<QuoteDetail> GetUniqueProductIdsWithMaxValueQuotes(List<QuoteDetail> existingQuoteDetails)
    {
        existingQuoteDetails = existingQuoteDetails.GroupBy(x => x.ProductId).Select(x => x.OrderByDescending(y => y.Value).First()).ToList();
        return existingQuoteDetails;
    }
}