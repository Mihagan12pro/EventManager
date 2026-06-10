namespace EventManager.Domain.ValueObjects.Events.DateAndTime
{
    public record DateTimeRange(
        DateTime? LowerBound,
        DateTime? UpperBound) : ValueObject;
}
