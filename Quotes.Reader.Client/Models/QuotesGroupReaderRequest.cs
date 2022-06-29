namespace Quotes.Reader.Client.Models
{
    public class QuotesGroupReaderRequest
    {
        public string StorageDirectory { get; set; }
        public string OutputDirectory { get; set; }
        public string AsOfDateString { get; set; }
        public string GroupName { get; set; }

        public QuotesGroupReaderRequest(string storageDirectory, string outputDirectory, string asOfDate, string groupName)
        {
            StorageDirectory = storageDirectory;
            OutputDirectory = outputDirectory;
            AsOfDateString = asOfDate;
            GroupName = groupName;
        }
    }
}
