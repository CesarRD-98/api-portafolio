using Cesardd.Shared.Exceptions;

namespace Cesardd.Core.Features.Contact.SendContact
{
    public class SendContactValidator
    {
        public void Validate(SendContactRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new AppException("El nombre es requerido");
            }

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                throw new AppException("El email es requerido");
            }

            if (!request.Email.Contains('@'))
            {
                throw new AppException("El email no es válido");
            }

            if (string.IsNullOrWhiteSpace(request.Message) || request.Message.Length < 10)
            {
                throw new AppException("El mensaje debe tener al menos 10 caracteres");
            }
        }
    }
}
