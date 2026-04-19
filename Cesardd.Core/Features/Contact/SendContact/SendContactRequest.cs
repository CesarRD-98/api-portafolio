namespace Cesardd.Core.Features.Contact.SendContact
{
    public class SendContactRequest
    {
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Message { get; set; } = default!;
    }
}
