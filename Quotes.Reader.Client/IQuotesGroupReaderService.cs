using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quotes.Reader.Client
{
    public interface IQuotesGroupReaderService
    {
        void ReadGroupQuotes(string storageDirectory, string outputDirectory, DateTime date, string groupName);
    }
}
