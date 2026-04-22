using System.ComponentModel.DataAnnotations;

namespace EventManager.DTOs.Events
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="Title">Events title. Required field</param>
    /// <param name="StartAt">Date time of start. Required field</param>
    /// <param name="EndAt">Date time of end. Required field</param>
    /// <param name="Description">Event discription. Optional field</param>
    public record NewEventDto(
        string Title,
        [Required] DateTime? StartAt,
        [Required] DateTime? EndAt,
        [Required] int? TotalSeats,
        string Description = "");
}
