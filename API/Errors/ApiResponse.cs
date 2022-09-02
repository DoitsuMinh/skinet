namespace API.Errors
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }


        //pass status code
        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A bad request, YOU have made",
                401 => "Authorized, YOU are not",
                404 => "Resouce found, IT was not",
                500 => "Appreciate for ignoring any inconvenience",
                _ => null
            };
        }
    }
}