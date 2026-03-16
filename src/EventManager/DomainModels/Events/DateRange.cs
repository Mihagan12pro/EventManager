using EventManager.Shared;

namespace EventManager.DomainModels.Events
{
    /// <summary>
    /// Provides date and time validation
    /// </summary>
    public class DateRange
    {
        /// <summary>
        /// Date and time can't be equal and lower than lower bound
        /// </summary>
        public readonly bool StrictlyGreater;

        /// <summary>
        /// Date and time can't be equal and upper than upper bound
        /// </summary>
        public readonly bool StrictlyLower;

        public readonly DateTime? LowerBound;

        public readonly DateTime? UpperBound;

        public Result CheckDateRange(DateTime dateTime)
        {
            if (LowerBound.HasValue)
            {
                if (StrictlyGreater && LowerBound >= dateTime)
                    return new Result(
                        false, 
                        "date time must be strictly greater than lower bound!"
                        );
                else if (!StrictlyGreater && LowerBound > dateTime)
                    return new Result(
                        false,
                        "date time must be greater than lower bound!"
                        );
            }

            if (UpperBound.HasValue)
            {
                if (StrictlyLower && UpperBound <= dateTime)
                    return new Result(
                        false,
                        "date time must be strictly smaller than lower bound!"
                        );
                else if (!StrictlyLower && UpperBound > dateTime)
                    return new Result(
                        false,
                        "date time must be lower than upper bound!"
                        );
            }

            return new Result(true, "Everything is ok!");
        }

        public DateRange(
            DateTime? LowerBound, 
            DateTime? UpperBound,
            bool StrictlyUpper = true,
            bool StrictlyLower = true)
        {
            this.LowerBound = LowerBound;
            this.UpperBound = UpperBound;

            this.LowerBound = LowerBound;
            this.UpperBound = UpperBound;
        }
    }
}
