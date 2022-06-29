namespace Quotes.Import.Service.Attributes
{
    /// <summary>
    /// An object holding config variables for Service Authentication
    /// </summary>
    public class ServiceAuthenticationConfig
    {
        /// <summary>
        /// The api key used for basic service authentication
        /// </summary>
        public string? ApiKey { get; set; }
    }
}
