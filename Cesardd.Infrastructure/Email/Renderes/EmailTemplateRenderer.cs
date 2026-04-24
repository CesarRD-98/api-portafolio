using System.Net;

namespace Cesardd.Infrastructure.Email.Renderes
{
    public class EmailTemplateRenderer
    {
        public static string Render(string template, Dictionary<string, string> values)
        {
            foreach (var item in values)
            {
                string safeValue;

                if (item.Key == "Message")
                {
                    safeValue = WebUtility.HtmlEncode(item.Value)
                        .Replace("\n", "<br>");
                }
                else
                {
                    safeValue = WebUtility.HtmlEncode(item.Value);
                }

                template = template.Replace($"{{{{{item.Key}}}}}", safeValue);
            }

            return template;
        }
    }
}