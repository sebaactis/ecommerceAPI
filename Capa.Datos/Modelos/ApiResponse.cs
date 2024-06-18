namespace Capa.Datos.Modelos
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public T Payload { get; set; }
        public string Message { get; set; }

        public ApiResponse(bool success, int statusCode, T payload, string message)
        {
            Success = success;
            StatusCode = statusCode;
            Payload = payload;
            Message = message;
        }

        public static ApiResponse<T> SuccessResponse(T payload, int statusCode, string? message = null)
        {
            return new ApiResponse<T>(true, statusCode, payload, message);
        }

        public static ApiResponse<T> ErrorResponse(int statusCode, string message)
        {
            return new ApiResponse<T>(false, statusCode, default(T), message);
        }
    }
}
