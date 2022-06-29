namespace Quotes.Reader.Client.ServiceModel
{
    public class ProductQuote
    {
        /// <summary>
        /// Unique of the product
        /// </summary>
        public string ProductId { get; set; }
        /// <summary>
        /// Quotes date
        /// </summary>
        public DateTime AsOfDate { get; set; }

        /// <summary>
        /// Quotes value
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// Quotes Group name
        /// </summary>
        public string GroupName { get; set; }
    }
}
