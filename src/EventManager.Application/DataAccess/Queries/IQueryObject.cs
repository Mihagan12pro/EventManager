namespace EventManager.Application.DataAccess.Queries
{
    public interface IQueryObject<TQueryBody> 
        where TQueryBody : IQueryBody
    {
        Task Execute(
            TQueryBody queryBody,
            CancellationToken cancellationToken);
    }




    public interface IQueryObject<TData, TQueryBody> 
        where TQueryBody : IQueryBody
    {
        Task<TData> Execute(
            TQueryBody queryBody, 
            CancellationToken cancellationToken);
    }
}
