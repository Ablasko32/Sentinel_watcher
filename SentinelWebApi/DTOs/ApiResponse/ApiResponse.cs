namespace SentinelWebApi.DTOs.ApiResponse
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public IEnumerable<string>? Error { get; set; }

        public static ApiResponse<T> ApiSuccess(T? data = default)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Data = data,
                Error = null
            };
        }

        public static ApiResponse<T> ApiError(IEnumerable<string> errorMessage)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Data = default,
                Error = errorMessage
            };
        }
    }
}