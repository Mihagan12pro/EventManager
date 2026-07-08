using Microsoft.Extensions.Options;
using Shared.Infrastracture.Kafka;
using Shared.Infrastracture.Kafka.Consumers;
using Shared.Messaging;
using Shared.Messaging.Contracts.Bookings;

namespace Events.Infrastracture.Consumers.Pending
{
    internal class PendingConsumer : KafkaConsumer<PendingBooking>
    {
        public PendingConsumer(
            IOptions<KafkaConsumerSettings> options,
            IMessageHandler<PendingBooking> messsageHandler) 
                : base(options, messsageHandler)
        {
        }
    }
}
