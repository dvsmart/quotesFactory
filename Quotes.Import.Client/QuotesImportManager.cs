public class QuotesImportManager : IQuotesImportManager
{
   private readonly ILogger<QuotesImportManager> _logger;
   public QuotesImportManager(ILogger<QuotesImportManager> logger)
   {
      _logger = logger; 
   }
   
   public void ProcessQuotes(string inputDirectory, string storageDirectory) 
   {
    try
    {
    
    }
    catch(Exception ex)
    {
      //_logger.
    }
   
   }


}