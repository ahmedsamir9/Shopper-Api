namespace ShopperAPi.Errors
{
    public class ApiException : ApiErrorResponse
    {
        public ApiException(int _statusCode, string message = null, string details = null) : base(_statusCode, message)
        {
            Details = details;
        }
        public string Details { get; set; }
    }
}
