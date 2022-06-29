using Newtonsoft.Json;
using Quotes.Import.Client.ServiceModels;

namespace Quotes.Import.Client.Extensions
{
    public static class QuotesFileWriterHelper
    {
        public static void WriteQuotesIntoFile(IEnumerable<IGrouping<string, ProductQuote>> quotesGroup,
            IEnumerable<ProductQuote> quotesToBeAdded,
            string storageDirectory)
        {
            foreach (var item in quotesGroup)
            {
                var path = storageDirectory + @$"\{item.Key}.json";

                // If matching json file exist then update the file with new quotes
                if (File.Exists(path))
                {
                    var jsonData = File.ReadAllText(path);
                    // De-serialize to object or create new list
                    var existingQuoteDetails = JsonConvert.DeserializeObject<List<ProductQuote>>(jsonData)
                                          ?? new List<ProductQuote>();

                     // Appending existing quotes and new quotes
                    existingQuoteDetails.AddRange(quotesToBeAdded);


                    // Filter unique product id with Max value and convert it into json data string
                    jsonData = JsonConvert.SerializeObject(GetUniqueProductIdsWithMaxValueQuotes(existingQuoteDetails));
                    
                    // Write it into a file.
                    File.WriteAllText(path, jsonData);
                }

                // If no matching file then add quotes into a new file.
                else
                {
                    var quotesToBeNewlyAdded = GetUniqueProductIdsWithMaxValueQuotes(item.ToList());
                    using (StreamWriter file = File.CreateText(path))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        //serialize object directly into file stream
                        serializer.Serialize(file, quotesToBeNewlyAdded);
                    }
                }
            }
        }

        static List<ProductQuote> GetUniqueProductIdsWithMaxValueQuotes(List<ProductQuote> existingQuoteDetails)
        {
            existingQuoteDetails = existingQuoteDetails.GroupBy(x => x.ProductId).Select(x => x.OrderByDescending(y => y.Value).First()).ToList();
            return existingQuoteDetails;
        }

    }
}
