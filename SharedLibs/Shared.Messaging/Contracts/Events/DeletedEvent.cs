namespace Shared.Messaging.Contracts.Events
{
    public class DeletedEvent : IMessage
    {
        public required Guid EventId { get; set; }
    }
}
