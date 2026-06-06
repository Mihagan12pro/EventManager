using System.Linq.Expressions;

namespace EventsManager.Shared.Filters
{
    public class Filters<T> : List<Expression<Func<T, bool>>>
    {
        public Filters()
        {
            
        }

        public Filters(params Expression<Func<T, bool>>[] filters)
        {
            foreach (var filter in filters)
            {
                Add(filter);
            }
        }

        public IQueryable<T> ApplyFilters(IQueryable<T> values)
        {
            foreach (var filter in this)
                values = values.Where(filter);

            return values;
        }

        public void Add(
            Expression<Func<T, bool>> filter,
            Func<bool> condition)
        {
            if (condition.Invoke())
                Add(filter);
        }
    }
}
