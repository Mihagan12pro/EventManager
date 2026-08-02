using System.Text;

namespace Shared.Objects.Classes
{
    public class JwtToken
    {
        public string Issuer { get; set; }

        public string ExpiredMinutes { get; set; }

        public byte[] IssuerSigningKey { get; set; }

        public IEnumerable<string> Audiences { get; set; }

        public string SecretKey
        {
            get => Encoding.UTF8.GetString(IssuerSigningKey);

            set => IssuerSigningKey = Encoding.UTF8.GetBytes(value);
        }
    }
}
