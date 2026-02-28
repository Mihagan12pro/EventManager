namespace EventManager.DTOs.Events
{
    public record NewEventDto(
        string Title,
        DateTime StartAt,
        DateTime EndAt, 
        string Description = "");
}
