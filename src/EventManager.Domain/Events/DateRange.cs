namespace EventManager.Domain.Events
{
    public record DateRange(
        DateTime? LowerBound,
        DateTime? UpperBound);
}
