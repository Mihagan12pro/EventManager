namespace EventManager.DTOs.Shared
{
    public record PaginationDto(
        int Page = 1, 
        int PageSize = 10);
}
