using Microsoft.Extensions.Configuration;
using System.Text;

namespace Shared.Objects.Classes.Options
{
    public class AuthOptions
    {
        public string Issuer { get; set; }
        public string SecretKey { get; set; }
        public string ExpiredMinutes { get; set; }

        public byte[] IssuerSigningKey { get; set; }

        public Dictionary<string, string> Audiences { get; set; }
    }
}
