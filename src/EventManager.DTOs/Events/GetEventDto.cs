namespace EventManager.DTOs.Events
{
    public record GetEventDto(
        string Title,
        DateTime StartAt, 
        DateTime EndAt,
        string Description);

}
