using FluentValidation;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quotes.Reader.Client.Models;
using Quotes.Reader.Client.ServiceModel;

namespace Quotes.Reader.Client.Services
{
    /// <summary>
    /// Quotes group reader service handles reading quotes from the storage location and stores the output json in to the given location.
    /// </summary>
    public class QuotesGroupReaderService : IQuotesGroupReaderService
    {
        private readonly ILogger<QuotesGroupReaderService> _logger;
        private readonly IValidator<QuotesGroupReaderRequest> _quotesGroupReaderRequest; 

        public QuotesGroupReaderService(ILogger<QuotesGroupReaderService> logger,
            IValidator<QuotesGroupReaderRequest> quotesGroupReaderRequest)
        { 
            _logger = logger;
            _quotesGroupReaderRequest = quotesGroupReaderRequest;
        }

        public async Task ProcessAsync(QuotesGroupReaderRequest groupReaderRequest)
        {

            // Check if the request object is valid.
            var validationResult = await _quotesGroupReaderRequest.ValidateAsync(groupReaderRequest);
            if (!validationResult.IsValid)
            {
                _logger.LogError(string.Join(",", validationResult.Errors));
                return;
            }
             
            // Check if output directory exists orelse create the directory.
            if (!Directory.Exists(@$"{groupReaderRequest.OutputDirectory}"))
            {
                _logger.LogWarning("Output directory doesnt exists.Hence Creating the directory");
                Directory.CreateDirectory(@$"{groupReaderRequest.OutputDirectory}");
            }

            // Check if storage directory exists
            if (!Directory.Exists(@$"{groupReaderRequest.StorageDirectory}"))
            {
                _logger.LogError("Invalid storage directory");
                return;
            }

            // Convert date string into datetime.
            _ = DateTime.TryParse(groupReaderRequest.AsOfDateString, out DateTime asOfDate);
            var expectedQuotesDate = asOfDate.ToString("dd/MM/yyyy");

            // Construct outpust json path.
            var outputJsonPath = groupReaderRequest.OutputDirectory +
                @$"\{groupReaderRequest.GroupName}-{asOfDate.ToString("yyyy-MM-dd")}.json";
            string? outputJsonData = null;
            try
            {
                var path = groupReaderRequest.StorageDirectory + @$"\{groupReaderRequest.GroupName}.json";
                
                // If the group quotes json already exists then construct the json into required output file format.
                if (File.Exists(path))
                {
                    var jsonData = File.ReadAllText(path);
                    // De-serialize to object
                    var existingQuoteDetails = JsonConvert.DeserializeObject<List<ProductQuote>>(jsonData)
                                          ?? new List<ProductQuote>();
                    existingQuoteDetails = existingQuoteDetails.Where(x => x.AsOfDate == asOfDate).ToList();

                    outputJsonData = ConstructOutputQuotesJson(groupReaderRequest.GroupName, expectedQuotesDate, existingQuoteDetails);
                }
                else
                {
                    //If the group quotes json file doesnt exists then construct the output json with empty quotes list.. 
                    outputJsonData = ConstructOutputQuotesJson(groupReaderRequest.GroupName, expectedQuotesDate, null);

                    _logger.LogInformation($"No quotes found for {groupReaderRequest.GroupName} from the: {groupReaderRequest.StorageDirectory} for {expectedQuotesDate}");
                }

                // Write output json into the output file path
                File.WriteAllText(outputJsonPath, outputJsonData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unable to {groupReaderRequest.GroupName} read quotes from the: {groupReaderRequest.StorageDirectory}");
            }
        }

        /// <summary>
        /// Construct quote list into the expected json format(i.e){ GroupName:"",AsofDate:"",Quotes:[{ProductId:"",AsOfDate:"", Value:""}]}
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="expectedQuotesDate"></param>
        /// <param name="existingQuoteDetails"></param>
        /// <returns></returns>
        private static string ConstructOutputQuotesJson(string groupName, string expectedQuotesDate, List<ProductQuote> existingQuoteDetails)
        {
            string? outputJsonData;
            var outputData = new
            {
                GroupName = groupName,
                AsOfDate = expectedQuotesDate,
                Quotes = existingQuoteDetails.Select(x => new
                {
                    x.ProductId,
                    AsOfDate = expectedQuotesDate,
                    x.Value
                })
            };
            outputJsonData = JsonConvert.SerializeObject(outputData);
            return outputJsonData;
        }
    }
}
