using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace EventManager.DTOs.Events
{
    public record GetEventDto(
        string Title,
        DateTime StartAt, 
        DateTime EndAt,
        string Description);

}
