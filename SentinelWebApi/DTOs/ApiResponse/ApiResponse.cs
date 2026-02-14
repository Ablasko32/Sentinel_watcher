namespace SentinelWebApi.DTOs.ApiResponse
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public object? Data { get; set; } = null;
        public string? Message { get; set; }

        public static ApiResponse ApiSuccess(string? message = null)
        {
            return new ApiResponse
            {
                Success = true,
                Data = null,
                Message = message
            };
        }

        public static ApiResponse ApiError(string? message = null)
        {
            return new ApiResponse
            {
                Success = false,
                Data = null,
                Message = message
            };
        }
    }

    public class ApiResponse<T> : ApiResponse
    {
        public new T? Data { get; set; }

        public static ApiResponse<T> ApiSuccess(T? data = default, string? message = null)
        {
            return new ApiResponse<T>
            {
                Success = true,
                Data = data,
                Message = message
            };
        }
    }
}