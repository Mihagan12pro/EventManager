namespace Events.Application.Dtos
{
    public record GetEventDto(
        Guid Id,
        
        string Title,
        
        DateTime StartAt,
        
        DateTime EndAt,
        
        string Description,
        
        int TotalSeats,

        int AvalibleSeats
    );
}
