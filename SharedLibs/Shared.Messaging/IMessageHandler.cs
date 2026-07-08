using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Messaging
{
    public interface IMessageHandler<in TMessage>
        where TMessage : IMessage
    {
        Task HandleAsync(
           TMessage message,
           CancellationToken cancellationToken);
    }
}
