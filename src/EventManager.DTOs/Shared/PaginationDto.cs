namespace EventManager.DTOs.Shared
{
    public record PaginationDto
    {
        public int Page { get; init; } 
        public int PageSize { get; init; }

        public int Skip { get; init; }

        public PaginationDto(int page, int pageSize)
        {
            Page = page;

            PageSize = pageSize;

            Skip = (Page - 1) * pageSize; 
        }
    }
}
