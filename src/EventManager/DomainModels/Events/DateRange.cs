using EventManager.Shared;

namespace EventManager.DomainModels.Events
{
    /// <summary>
    /// Provides start and end date times validation
    /// </summary>
    public class DateRange
    {
        /// <summary>
        /// Date and time can't be equal and lower than lower bound
        /// </summary>
        public readonly bool StrictlyGreater = true;

        /// <summary>
        /// Date and time can't be equal and upper than upper bound
        /// </summary>
        public readonly bool StrictlyLower = true;

        public readonly DateTime? LowerBound;

        public readonly DateTime? UpperBound;

        public Result CheckDateRange(Event eventModel)
        {
            if (LowerBound.HasValue)
            {
                if (StrictlyGreater && LowerBound >= eventModel.StartAt)
                    return new Result(
                        false, 
                        "date time must be strictly greater than lower bound!"
                        );
                else if (!StrictlyGreater && LowerBound > eventModel.StartAt)
                    return new Result(
                        false,
                        "date time must be greater than lower bound!"
                        );
            }

            if (UpperBound.HasValue)
            {
                if (StrictlyLower && UpperBound <= eventModel.EndAt)
                    return new Result(
                        false,
                        "date time must be strictly smaller than lower bound!"
                        );
                else if (!StrictlyLower && UpperBound < eventModel.EndAt)
                    return new Result(
                        false,
                        "date time must be lower than upper bound!"
                        );
            }

            return new Result(true, "Everything is ok!");
        }

        public DateRange(
            DateTime? LowerBound,
            bool? StrictlyGreater,
            DateTime? UpperBound,
            bool? StrictlyLower)
        {
            this.LowerBound = LowerBound;
            this.UpperBound = UpperBound;

            if (StrictlyGreater.HasValue)
                this.StrictlyGreater = StrictlyGreater.Value;

            if (StrictlyLower.HasValue)
                this.StrictlyLower = StrictlyLower.Value;
        }
    }
}
