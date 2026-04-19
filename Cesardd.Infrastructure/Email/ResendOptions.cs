namespace Cesardd.Infrastructure.Email
{
    public class ResendOptions
    {
        public string ApiKey { get; set; } = default!;
        public string FromEmail { get; set; } = default!;
        public string ContactEmail { get; set; } = default!;
    }
}
