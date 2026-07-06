using Confluent.Kafka;
using Events.API.Contracts;
using Shared.Infrastracture.Kafka.Consumers;

namespace Events.API.MessageHandlers
{
    public class JwtMessageHandler : IMessageHandler<JwtTokenContract>
    {
        public async Task HandleAsync(JwtTokenContract message, CancellationToken cancellationToken)
        {
           
        }
    }
}
