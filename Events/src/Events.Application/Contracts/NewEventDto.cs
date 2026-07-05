using System.ComponentModel.DataAnnotations;

namespace Events.Application.Contracts
{
    public record NewEventDto(
        string Title,
        [Required] DateTime? StartAt,
        [Required] DateTime? EndAt,
        [Required] int? TotalSeats,
        string Description = "");
}
