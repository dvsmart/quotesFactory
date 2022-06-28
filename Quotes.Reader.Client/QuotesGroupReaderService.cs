using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quotes.Import.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quotes.Reader.Client
{
    internal class QuotesGroupReaderService : IQuotesGroupReaderService
    {
        private readonly ILogger<QuotesGroupReaderService> _logger;
        public QuotesGroupReaderService(ILogger<QuotesGroupReaderService> logger)
        {
            _logger = logger;
        }

        public void ReadGroupQuotes(string storageDirectory, string outputDirectory, DateTime date, string groupName)
        {
            if (string.IsNullOrEmpty(outputDirectory) || !Directory.Exists(@$"{outputDirectory}"))
            {
                _logger.LogError("Output directory doesnt exists.Hence Creating the directory"); 
                Directory.CreateDirectory(@$"{outputDirectory}"); 
            }

            if (string.IsNullOrEmpty(storageDirectory) || !Directory.Exists(@$"{storageDirectory}"))
            {
                _logger.LogError("Invalid storage directory");
                return;
            }

            if (date == default(DateTime))
            {
                _logger.LogError("Invalid date");
                return;
            }

            if (string.IsNullOrEmpty(groupName))
            {
                _logger.LogError("Invalid group name");
                return;
            }
            // Update json data string
            var expectedQuotesDate = date.ToString("dd/MM/yyyy");
            var outputJsonPath = outputDirectory + @$"\{groupName}-{date.ToString("yyyy-MM-dd")}.json";
            string? outputJsonData = null;
            try
            { 
                var path = storageDirectory + @$"\{groupName}.json";
                if (File.Exists(path))
                {
                    var jsonData = File.ReadAllText(path);
                    // De-serialize to object or create new list
                    var existingQuoteDetails = JsonConvert.DeserializeObject<List<QuoteDetail>>(jsonData)
                                          ?? new List<QuoteDetail>();
                    existingQuoteDetails = existingQuoteDetails.Where(x => x.AsOfDate == date).ToList();

                    outputJsonData = ConstructOutputQuotesJson(groupName, expectedQuotesDate, existingQuoteDetails);
                }
                else
                {
                    outputJsonData = ConstructOutputQuotesJson(groupName, expectedQuotesDate, null);
 
                    _logger.LogInformation($"No quotes found for {groupName} from the: {storageDirectory} for {expectedQuotesDate}");
                }
                File.WriteAllText(outputJsonPath, outputJsonData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to {groupName} read quotes from the: {storageDirectory}");
            } 
        }

        private static string ConstructOutputQuotesJson(string groupName, string expectedQuotesDate, List<QuoteDetail> existingQuoteDetails)
        {
            string? outputJsonData;
            var outputData = new
            {
                GroupName = groupName,
                AsOfDate = expectedQuotesDate,
                Quotes = existingQuoteDetails.Select(x => new
                {
                    ProductId = x.ProductId,
                    AsOfDate = expectedQuotesDate,
                    Value = x.Value
                })
            };
            outputJsonData = JsonConvert.SerializeObject(outputData);
            return outputJsonData;
        }
    }
}
