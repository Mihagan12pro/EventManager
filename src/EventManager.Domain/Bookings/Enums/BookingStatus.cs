namespace EventManager.Domain.Bookings.Enums
{
    public enum BookingStatus
    {
        /// <summary>
        /// Booking had been created. It is awaiting of handling
        /// </summary>
        Pending,

        /// <summary>
        /// Booking had been confirmed
        /// </summary>
        Confirmed,

        /// <summary>
        /// Booking had been rejected
        /// </summary>
        Rejected
    }
}
