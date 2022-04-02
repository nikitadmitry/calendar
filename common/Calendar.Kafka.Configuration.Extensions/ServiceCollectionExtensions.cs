using Calendar.Agenda.Domain.Entities.Messages;
using Calendar.Kafka.Abstractions;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Calendar.Kafka.Configuration.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddKafkaConsumer<THandler, TMessage>(this IServiceCollection services)
            where THandler : class, IMessageHandler<TMessage>
            where TMessage : IMessage
        {
            services.AddSingleton<IMessageHandler<TMessage>, THandler>();

            return services.AddHostedService(sp =>
            {
                var options = sp.GetRequiredService<IOptions<KafkaConsumerOptions>>().Value;

                var config = new ConsumerConfig
                {
                    BootstrapServers = options.Servers,
                    GroupId = options.ConsumerGroupId,
                    AutoOffsetReset = AutoOffsetReset.Earliest
                };

                var consumer = new ConsumerBuilder<Ignore, string>(config).Build();

                return new KafkaConsumer<TMessage>(options.Topics[typeof(TMessage).Name], consumer,
                    sp.GetRequiredService<IMessageHandler<TMessage>>(),
                    sp.GetRequiredService<ILogger<KafkaConsumer<TMessage>>>());
            });
        }
    }
}