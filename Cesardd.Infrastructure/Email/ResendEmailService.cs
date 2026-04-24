using Cesardd.Core.Interfaces;
using Cesardd.Infrastructure.Email.Renderes;
using Cesardd.Shared.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Resend;

namespace Cesardd.Infrastructure.Email
{
    public class ResendEmailService(
                IResend resend, IOptions<ResendOptions> options, ILogger<ResendEmailService> logger
            ) : IEmailService
    {
        private readonly IResend _resend = resend;
        private readonly ILogger<ResendEmailService> _logger = logger;
        private readonly ResendOptions _options = options.Value;

        public async Task SendEmailContactAsync(string name, string email, string message)
        {
            try
            {
                var templatePath = Path.Combine(
                    AppContext.BaseDirectory,
                    "Email",
                    "Templates",
                    "ContactEmailTemplate.html"
                );

                var template = File.ReadAllText(templatePath);

                var html = EmailTemplateRenderer.Render(template, new Dictionary<string, string>
                {
                    { "Name", name },
                    { "Email", email },
                    { "Message", message }
                });

                var response = await _resend.EmailSendAsync(new EmailMessage
                {
                    From = _options.FromEmail,
                    To = _options.ContactEmail,
                    Subject = $"Nuevo mensaje de {name}",
                    HtmlBody = html
                });

                if (!response.Success)
                {
                    if (_logger.IsEnabled(LogLevel.Warning))
                    {
                        _logger.LogWarning("Resend falló al enviar correo para {Email}", email);
                    }

                    throw new AppException("No se pudo enviar el correo", 500);
                }

                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Correo enviado correctamente desde {Email}", email);
                }
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError(ex, "Error inesperado enviando correo desde {Email}", email);
                }

                throw new AppException("Error interno al enviar el correo", 500);
            }
        }
    }
}
