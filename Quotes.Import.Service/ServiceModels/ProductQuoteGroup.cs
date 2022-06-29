namespace Quotes.Import.Service.ServiceModels
{
    public class ProductQuoteGroup
    {
        /// <summary>
        /// Unique name of the Quotes group
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Unique id of the product
        /// </summary>
        public string ProductId { get; set; }
    }
}
