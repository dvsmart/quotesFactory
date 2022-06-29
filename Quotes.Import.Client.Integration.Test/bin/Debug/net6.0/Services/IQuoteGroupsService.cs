using Quotes.Common.Contracts;
using Quotes.Import.Service.ServiceModels;

namespace Quotes.Import.Service.Services
{
    public interface IQuoteGroupsService
    {
        /// <summary>
        /// Get Quote Group mappings
        /// </summary>
        /// <returns></returns>
        Result<List<ProductQuoteGroup>> GetQuoteGroups(string groupMappingFilePath);
    }
}
