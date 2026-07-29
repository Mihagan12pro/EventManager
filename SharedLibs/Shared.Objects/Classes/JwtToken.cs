namespace Shared.Objects.Classes
{
    public class JwtToken
    {
        public string Issuer { get; set; }
        public string SecretKey { get; set; }
        public string ExpiredMinutes { get; set; }

        public byte[] IssuerSigningKey { get; set; }

        public IEnumerable<string> Audiences { get; set; }
    }
}
