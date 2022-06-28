namespace Quotes.Import.Client
{
    public sealed class QuotesGroupMapperService : IQuotesGroupMapperService
    {
        private static readonly Lazy<QuotesGroupMapperService> lazy = new Lazy<QuotesGroupMapperService>(() => new QuotesGroupMapperService());
        public static QuotesGroupMapperService Instance { get { return lazy.Value; } }

        public QuotesGroupMapperService()
        {

        }

        public List<QuotesGroupDetails> GetQuotesGroupDetails()
        {
            string fileName = Path.GetFullPath(Directory.GetCurrentDirectory() + @"\GroupMapping.csv");
             
            var quotesGroupDetails = new List<QuotesGroupDetails>();
            using (StreamReader sr = File.OpenText(fileName))
            {
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    var row = line.Split(',');
                    if (row == null || row.Length < 2)
                    {
                        throw new ArgumentOutOfRangeException("The Group mapping csv file is invalid.");
                    }

                    quotesGroupDetails.Add(new QuotesGroupDetails
                    {
                        GroupName = row[0],
                        ProductId = row[1] 
                    });
                }
            }

            return quotesGroupDetails;
        }

    }
}
