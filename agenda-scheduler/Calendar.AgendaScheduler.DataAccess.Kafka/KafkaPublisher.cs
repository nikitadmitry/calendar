using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Calendar.Agenda.Domain.Entities.Messages;
using Calendar.AgendaScheduler.DataAccess.Interfaces;
using Confluent.Kafka;

namespace Calendar.AgendaScheduler.DataAccess.Kafka
{
    public class KafkaPublisher : IMessagePublisher
    {
        private readonly IProducer<Null, string> _producer;
        private readonly KafkaTopicsProvider _kafkaTopicsProvider;

        public KafkaPublisher(IProducer<Null, string> producer, KafkaTopicsProvider kafkaTopicsProvider)
        {
            _producer = producer;
            _kafkaTopicsProvider = kafkaTopicsProvider;
        }

        public Task PublishAsync<T>(T message, CancellationToken cancellationToken) where T : IMessage
        {
            var topic = _kafkaTopicsProvider.Get<T>();
            return _producer.ProduceAsync(topic, new Message<Null, string>
            {
                Value = JsonSerializer.Serialize(message)
            }, cancellationToken);
        }
    }
}