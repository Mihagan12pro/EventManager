using Shared.Objects.Interfaces;
using Shared.Objects.Records;
using System.Globalization;


namespace Events.Application.Handlers.GetEventsCommand
{
    public record GetEventsCommand : ICommand
    {
        public string? Title { get; init; }

        public DateTime? From { get; init; }

        public DateTime? To { get; init; }

        public Pagination Pagination { get; init; }

        public GetEventsCommand(
            string? Title,
            DateTime? From,
            DateTime? To,
            Pagination Pagination)
        {
            this.To = To;
            this.Title = Title;
            this.From = From;
            this.Pagination = Pagination;

            if (this.Title != null)
                this.Title = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(this.Title);
        }
    };
}
