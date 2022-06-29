namespace Quotes.Import.Client.Models
{
    public class QuotesImportRequest
    {
        public string StorageDirectory { get; set; }
        public string InputDirectory { get; set; }

        public QuotesImportRequest(string inputDirectory, string storageDirectory)
        {
            StorageDirectory = storageDirectory;
            InputDirectory = inputDirectory;
        }
    }
}
