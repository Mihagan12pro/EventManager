using System.Security.Cryptography;
using System.Text;
using Users.Application.Security;

namespace Users.Infrastructure.Security
{
    internal class PasswordHasherSHA256 : IPasswordHasher
    {
        public string Hash(string password)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));

            return Convert.ToHexString(bytes);
        }
    }
}
