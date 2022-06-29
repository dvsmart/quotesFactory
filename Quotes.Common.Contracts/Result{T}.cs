namespace Quotes.Common.Contracts
{
    public class Result<T> where T : class
    { 
        // ErrorMessage
        public string ErrorMessage {get;set;}

        // Indicates result status
        public ResultStatus ResultStatus {get;set;} 

        // Indicates if its successfull or not
        public bool IsSuccessful => ResultStatus == ResultStatus.Success;

        // Actual data
        public T Data { get; set; }

        public Result()
        {
            ResultStatus = ResultStatus.Success; 
        }
    }
}
