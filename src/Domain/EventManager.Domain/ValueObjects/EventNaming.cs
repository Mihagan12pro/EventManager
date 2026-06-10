using EventManager.Domain.Failures.Exceptions.WebApi.Client.BadRequest;
using System.Globalization;

namespace EventManager.Domain.ValueObjects
{
    public record EventNaming
    {
        public string Title
        {
            get
            {
                return _title;
            }
            init
            {
                if (value.Length < 1)
                    throw new BadRequestException("Too short title!");

                _title = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(value);
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
            init
            {
                _description = value;
            }
        }


        public EventNaming(string title, string description = "")
        {
            Title = title;

            Description = description;
        }

        private string _title, _description;
    }
}
