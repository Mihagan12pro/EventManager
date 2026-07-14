namespace Shared.Messaging
{
    /// <summary>
    /// For inbox tables
    /// </summary>
    public class Message : IMessage
    {
        public required Guid Id { get; set; }
    }
}
