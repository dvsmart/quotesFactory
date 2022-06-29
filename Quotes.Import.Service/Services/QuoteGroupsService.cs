using Quotes.Common.Contracts;
using Quotes.Import.Service.ServiceModels;

namespace Quotes.Import.Service.Services
{
    public sealed class QuoteGroupsService : IQuoteGroupsService
    {
        private static readonly Lazy<QuoteGroupsService> lazy = new Lazy<QuoteGroupsService>(() => new QuoteGroupsService());
        public static QuoteGroupsService Instance { get { return lazy.Value; } } 

        /// <inheritdoc />
        public Result<List<ProductQuoteGroup>> GetQuoteGroups(string groupMappingFilePath)
        { 
            var quotesGroupDetails = new List<ProductQuoteGroup>();
            var hasReadHeader = false;
            try
            {
                using (var sr = File.OpenText(groupMappingFilePath))
                {
                    string line = "";
                    while ((line = sr?.ReadLine()) != null)
                    {
                        if (!hasReadHeader)
                        {
                            hasReadHeader = true;
                            continue;
                        }

                        var row = line.Split(',');
                        if (row == null || row.Length < 2)
                        {
                            throw new ArgumentOutOfRangeException("The Group mapping csv file is invalid.");
                        }

                        quotesGroupDetails.Add(new ProductQuoteGroup
                        {
                            GroupName = row[0],
                            ProductId = row[1]
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                return new Result<List<ProductQuoteGroup>>() { 
                     ResultStatus = ResultStatus.GenericError,
                     ErrorMessage = ex.ToString()
                };
            }
            
            return new Result<List<ProductQuoteGroup>>()
            {
                Data = quotesGroupDetails
            }; ;
        }

    }
}
