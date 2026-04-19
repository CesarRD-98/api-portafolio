using Cesardd.Shared.Exceptions;

namespace Cesardd.Core.Features.Contact.SendContact
{
    public class SendContactValidator
    {
        public static void Validate(SendContactRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name)) { Throw("El nombre es requerido"); }
            if (string.IsNullOrWhiteSpace(request.Email)) { Throw("El email es requerido"); }
            if (!request.Email.Contains('@')) { Throw("El email no es válido"); }
            if (string.IsNullOrWhiteSpace(request.Message) || request.Message.Length < 10)
            {
                Throw("El mensaje debe tener al menos 10 caracteres");
            }
        }

        private static void Throw(string message)
        {
            throw new AppException(message);
        }
    }
}
