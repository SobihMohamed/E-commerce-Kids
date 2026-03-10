namespace E_commerce.Shared.Common.Responses
{
    public class ApiResponse<TData>
    {
        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public TData? Data { get; set; }
        public List<string>? Errors { get; set; } = new List<string>();

        public ApiResponse(TData data, string message, int statusCode = 200)
        {
            IsSuccess = true;
            Message = message;
            Data = data;
            StatusCode = statusCode;
        }
        public ApiResponse( string message, List<string> errors,  int statusCode = 400)
        {
            IsSuccess = false;
            Message = message;
            StatusCode = statusCode;
            Errors = errors;
        }


    }
}
