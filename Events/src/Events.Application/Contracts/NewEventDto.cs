using System.ComponentModel.DataAnnotations;

namespace Events.Application.Contracts
{
    public record NewEventDto(
        [Required, Length(3, 256)] string Title,
        [Required] DateTime? StartAt,
        [Required] DateTime? EndAt,
        [Required, Range(1, int.MaxValue)] int? TotalSeats,
        string Description = "");
}
