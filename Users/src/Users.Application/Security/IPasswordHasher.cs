using System;
using System.Collections.Generic;
using System.Text;

namespace Users.Application.Security
{
    public interface IPasswordHasher
    {
        string Hash(string password);
    }
}
