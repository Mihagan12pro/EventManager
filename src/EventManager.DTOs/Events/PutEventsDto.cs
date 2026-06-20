using System.ComponentModel.DataAnnotations;

namespace EventManager.DTOs.Events
{
    public record PutEventDto(string Title,
        [Required] DateTime? StartAt,
        [Required] DateTime? EndAt,
        string Description = "");
}
