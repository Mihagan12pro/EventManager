using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace EventManager.DTOs.Events
{
    public record NewEventDto(
        string Title,
        [Required]DateTime? StartAt,
        [Required]DateTime? EndAt, 
        string Description = "");
}
