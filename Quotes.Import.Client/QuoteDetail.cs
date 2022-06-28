using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quotes.Import.Client
{
    public class QuoteDetail
    {
        public string ProductId { get; set; }
        public DateTime AsOfDate { get; set; }
        public decimal Value { get; set; }
        public string GroupName { get; set; }
    }
}
