using System;
using System.Collections.Generic;
using System.Text;

namespace Bookings.Application.Repositories
{
    public interface IBookingRepository
    {
        Task CreateAsync(
            Guid eventId, 
            CancellationToken cancellationToken);
    }
}
