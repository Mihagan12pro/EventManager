namespace EventManager.Shared
{
    public class DateSpan
    {
        public int Year { get; }

        public int Month { get; }

        public int Day { get; }

        public DateSpan(DateTime dateTime1, DateTime dateTime2)
        {
            Year = dateTime1.Year - dateTime2.Year;
            Month = dateTime1.Month - dateTime2.Month;
            Day = dateTime1.Day - dateTime2.Day;
        }
    }
}
