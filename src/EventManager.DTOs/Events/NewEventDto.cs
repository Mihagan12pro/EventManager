using System.ComponentModel.DataAnnotations;

namespace EventManager.DTOs.Events
{
    public record NewEventDto(
        string Title,
        [Required]DateTime? StartAt,
        [Required]DateTime? EndAt, 
        string Description = "");
}
