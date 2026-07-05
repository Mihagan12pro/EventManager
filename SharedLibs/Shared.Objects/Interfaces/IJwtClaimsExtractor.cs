using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Objects.Interfaces
{
    public interface IJwtClaimsExtractor
    {
        string Extract(string name);
    }
}
