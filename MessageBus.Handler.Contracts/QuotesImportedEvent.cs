namespace MessageBus.Handler.Contracts
{
    public class QuotesImportedEvent: IQuotesEvent
    {
        public string StorageLocation { get; private set; } 
        public QuotesImportedEvent(string storageLocation)
        {
            StorageLocation = storageLocation;
        }
    } 
}