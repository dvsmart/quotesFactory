using Quotes.Import.Client.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quotes.Import.Client.Extensions
{
    public static class QuotesFileReaderHelper
    {
        public static IEnumerable<ProductQuote> ConstructQuoteDetails(string file)
        {
            var sr = new StreamReader(file);
            var quoteDetails = new List<ProductQuote>();
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
                    quoteDetails.Add(new ProductQuote
                    {
                        ProductId = row[0],
                        AsOfDate = DateTime.Parse(row[1]),
                        Value = string.IsNullOrWhiteSpace(row[2]) ? 0 : Convert.ToDecimal(row[2])
                    });
                }

            }
            return quoteDetails;
        }

    }
}
