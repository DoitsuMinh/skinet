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
                400 => "Suffering from a BAD request T.T",
                401 => "Authorization failed, YOU shall not pass X_X",
                404 => "Resouce found, NOTHING",
                500 => "Appreciate for IGNORING any inconvenience",
                _ => null
            };
        }
    }
}