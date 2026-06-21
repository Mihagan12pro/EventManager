using System.Text.Json.Serialization;

namespace EventManager.Domain.Entities.Users.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Roles
    {
        User,

        Admin
    }
}
