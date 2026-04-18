namespace Cesardd.Shared.Results
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string? Message { get; set; }

        public static ApiResponse<T> Ok(T data, string? message = null)
        {
            return new() { Success = true, Data = data, Message = message };
        }

        public static ApiResponse<T> Fail(string message)
        {
            return new() { Success = false, Message = message };
        }
    }
}
