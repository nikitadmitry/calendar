using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Calendar.Agenda.Domain.Entities.Messages;
using Calendar.Kafka.Abstractions;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Calendar.Kafka.Configuration.Extensions
{
    public class KafkaConsumer<T> : IHostedService, IDisposable where T : IMessage
    {
        private readonly string _topic;
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly IMessageHandler<T> _messageHandler;
        private readonly ILogger<KafkaConsumer<T>> _logger;
        private Task? _task;

        public KafkaConsumer(string topic, IConsumer<Ignore, string> consumer,
            IMessageHandler<T> messageHandler,
            ILogger<KafkaConsumer<T>> logger)
        {
            _topic = topic;
            _consumer = consumer;
            _messageHandler = messageHandler;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _task = Task.Factory.StartNew(() => Run(cancellationToken), cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            return Task.CompletedTask;
        }

        private async Task Run(CancellationToken cancellationToken)
        {
            _consumer.Subscribe(_topic);

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(cancellationToken);

                    var message = JsonSerializer.Deserialize<T>(consumeResult.Message.Value);

                    await _messageHandler.HandleAsync(message!, cancellationToken);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error while consuming message");
                }
            }

            _consumer.Close();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                _task?.Wait(cancellationToken);
            }
            catch (AggregateException ex)
            {
                _logger.LogInformation("Task canceled");
            }

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _consumer.Dispose();
        }
    }
}