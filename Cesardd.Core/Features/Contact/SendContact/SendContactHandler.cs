using Cesardd.Core.Interfaces;
using Cesardd.Shared.Results;

namespace Cesardd.Core.Features.Contact.SendContact
{
    public class SendContactHandler(IEmailService emailService)
    {
        private readonly IEmailService _emailService = emailService;

        public async Task<Result<SendContactResponse>> Handle(SendContactRequest request)
        {
            var validator = new SendContactValidator();

            validator.Validate(request);

            await _emailService.SendEmailContactAsync(request.Name, request.Email, request.Message);

            return Result<SendContactResponse>.Success(new()
            {
                Message = "Mensaje enviado correctamente"
            });
        }
    }
}
