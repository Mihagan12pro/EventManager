using EventManager.Domain.Failures.Errors;
using EventManager.Domain.Failures.Exceptions.WebApi.Client.BadRequest;
using EventsManager.Failures.Errors;

namespace EventManager.Domain.ValueObjects
{
    public record Pagination 
    {
        public int Page { get; init; }
        public int PageSize { get; init; }

        public int Skip { get; init; }

        public Pagination(int page, int pageSize)
        {
            Page = page;

            PageSize = pageSize;

            ErrorsCollection errors = new ErrorsCollection();

            if (Page <= 0)
                errors.Add(new Error("Page must be greater than zero!"));

            if (PageSize <= 0)
                errors.Add(new Error("Page size must be greater than zero!"));

            if (errors.HasErrors)
                throw new BadRequestException(errors);

            Skip = (Page - 1) * pageSize;
        }
    }
}
