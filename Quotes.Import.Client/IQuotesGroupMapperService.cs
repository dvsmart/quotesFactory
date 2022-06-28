using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quotes.Import.Client
{
    public interface IQuotesGroupMapperService
    {
        List<QuotesGroupDetails> GetQuotesGroupDetails();
    }
}
