using Cesardd.Core.Interfaces;
using Cesardd.Infrastructure.Email.Renderes;
using Microsoft.Extensions.Options;
using Resend;

namespace Cesardd.Infrastructure.Email
{
    public class ResendEmailService(
            IResend resend, IOptions<ResendOptions> options, EmailTemplateRenderer renderer
        ) : IEmailService
    {
        private readonly IResend _resend = resend;
        private readonly ResendOptions _options = options.Value;
        private readonly EmailTemplateRenderer _renderer = renderer;

        public async Task SendEmailContactAsync(string name, string email, string message)
        {
            var templatePath = Path.Combine(AppContext.BaseDirectory,
                "Email",
                "Templates",
                "ContactEmailTemplate.html"
            );

            var template = File.ReadAllText(templatePath);

            var html = _renderer.Render(template, new Dictionary<string, string>
            {
                { "Name", name },
                { "Email", email },
                { "Message", message.Replace("\n", "<br>") }
            });

            var response = await _resend.EmailSendAsync(new EmailMessage
            {
                From = _options.FromEmail,
                To = _options.ContactEmail,
                Subject = "Nuevo mensaje de contacto",
                HtmlBody = html
            });

            if (!response.Success)
            {
                throw new Exception("Error enviando email con el servidor de emails");
            }
        }
    }
}
