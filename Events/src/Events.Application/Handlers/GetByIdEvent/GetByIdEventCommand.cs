using Shared.Objects.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Events.Application.Handlers.GetByIdEvent
{
    public record GetByIdEventCommand(Guid Id) : ICommand;
}
