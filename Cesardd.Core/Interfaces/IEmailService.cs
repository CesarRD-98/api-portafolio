namespace Cesardd.Core.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailContactAsync(string name, string email, string message);
    }
}
