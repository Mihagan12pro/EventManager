using EventManager.Application.Security;
using System.Security.Cryptography;
using System.Text;

namespace EventManager.Infrastructure.Security
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
