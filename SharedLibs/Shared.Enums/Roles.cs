using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Shared.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Roles
    {
        User,

        Admin
    }
}
