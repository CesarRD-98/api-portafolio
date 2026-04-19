namespace Cesardd.Shared.Results
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public ApiError? Error { get; set; }

        public static ApiResponse<T> Ok(T data)
        {
            return new() { Success = true, Data = data, Error = null };
        }

        public static ApiResponse<T> Fail(string message)
        {
            return new() { Success = false, Error = new ApiError { Message = message } };
        }
    }
}
