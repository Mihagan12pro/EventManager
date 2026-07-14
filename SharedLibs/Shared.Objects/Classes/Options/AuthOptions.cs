using Microsoft.Extensions.Configuration;
using System.Text;

namespace Shared.Objects.Classes.Options
{
    public class AuthOptions : GlobalOptions
    {
        private readonly IConfigurationSection _jwtOptions;

        public readonly string Issuer;
        public readonly string SecretKey;
        public readonly string ExpiredMinutes;

        public readonly byte[] IssuerSigningKey;


        private readonly List<string> _audiences;
        

        public IReadOnlyList<string> Audiences
            => _audiences.AsReadOnly();

        public AuthOptions()
        {
            _jwtOptions = globalConfiguration.GetRequiredSection("JwtOptions");

            Issuer = _jwtOptions.GetRequiredSection("Issuer").Value;
            SecretKey = _jwtOptions.GetRequiredSection("SecretKey").Value;
            ExpiredMinutes = _jwtOptions.GetRequiredSection("ExpiredMinutes").Value;
            IssuerSigningKey = Encoding.UTF8.GetBytes(_jwtOptions.GetRequiredSection("SecretKey").Value);

            _audiences = new List<string>();

            foreach (var section in _jwtOptions.GetRequiredSection("Audiences").GetChildren())
                _audiences.Add(section.Value);
        }
    }
}
