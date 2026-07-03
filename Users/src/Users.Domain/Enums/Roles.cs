using System.Text.Json.Serialization;

namespace Users.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Roles
    {
        User,

        Admin
    }
}
