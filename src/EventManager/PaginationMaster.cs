using EventManager.DTOs.Shared;

namespace EventManager.Shared
{
    public static class PaginationMaster<T>
    {
        public static IEnumerable<T> DoPagination(
            IEnumerable<T> collection, 
            PaginationDto paginationParameters)
        {
            return collection.Skip((paginationParameters.Page - 1) * paginationParameters.PageSize)
                .Take(paginationParameters.PageSize);
        }
    }
}
