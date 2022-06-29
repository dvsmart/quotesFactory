namespace Quotes.Common.Contracts
{
    public class Result
    {
        // ErrorMessage
        public string ErrorMessage { get; set; }

        // Indicates result status
        public ResultStatus ResultStatus { get; set; }

        // Indicates if its successfull or not
        public bool IsSuccessful => ResultStatus == ResultStatus.Success;

    }
}
