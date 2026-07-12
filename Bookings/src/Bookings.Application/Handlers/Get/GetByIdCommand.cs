using Shared.Objects.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bookings.Application.Handlers.Get
{
    public record GetByIdCommand(Guid BookingId) : ICommand;
}
