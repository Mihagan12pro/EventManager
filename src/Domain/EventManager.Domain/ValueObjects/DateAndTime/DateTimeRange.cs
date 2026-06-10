namespace EventManager.Domain.ValueObjects.DateAndTime
{
    public record DateTimeRange(
        DateTime? LowerBound,
        DateTime? UpperBound);
}
