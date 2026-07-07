using System.ComponentModel.DataAnnotations;

namespace Events.Application.Dtos
{
    public record UpdateEventDto
    {
        public string? Title { get; init; }

        public string? Description { get; init; }
        
        public DateTime? From { get; init; }
        
        public DateTime? To { get; init; }

        public UpdateEventDto(
            [Length(3, 256)] string? Title,
            string? Description,
            DateTime? From,
            DateTime? To
            )
        {
            this.Description = Description;

            this.Title = Title;

            this.To = To;

            this.From = From;
        }
    }
}
