namespace ShopperAPi.Errors
{
    public class ApiErrorResponse
    {
        public ApiErrorResponse(int _statusCode ,string message = null)
        {
            StatusCode = _statusCode;
            Message = message ?? getDefualtMessageForCode(_statusCode);
        }

        private string getDefualtMessageForCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A bad request Happen",
                401 => "you Are Not Authorized",
                404 => "Resource Not Found",
                500 => "Server Error",
                _ => "UnKnownError"
            };
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}
