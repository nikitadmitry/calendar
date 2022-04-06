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
        private readonly CancellationTokenSource _cancellationSource = new CancellationTokenSource();

        public KafkaConsumer(string topic,
            IConsumer<Ignore, string> consumer,
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
            _task = Task.Factory.StartNew(Run, _cancellationSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            return Task.CompletedTask;
        }

        private async Task Run()
        {
            try
            {
                _consumer.Subscribe(_topic);
                while (!_cancellationSource.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = _consumer.Consume(_cancellationSource.Token);

                        if (!consumeResult.IsPartitionEOF)
                        {
                            var message = JsonSerializer.Deserialize<T>(consumeResult.Message.Value);
                            await _messageHandler.HandleAsync(message!, _cancellationSource.Token);
                        }
                    }
                    catch (ConsumeException ex)
                    {
                        if (ex.Error?.IsFatal ?? false)
                            throw;

                        _logger.LogError(ex, "Consumer failed with ConsumeException. Reason: " + ex.Error?.Reason);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Operation cancel exception");
            }
            catch (ConsumeException ex)
            {
                _logger.LogError("Consumer failed with ConsumeException. Reason: {ex.Error?.Reason}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError("Consumer failed on consume", ex);
            }
            finally
            {
                _consumer?.Close();
                _logger.LogInformation("Stopping consumer");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cancellationSource.Cancel();
            _logger.LogInformation("Status of task:" + _task?.Status);
            try
            {
                _task?.Wait();
            }
            catch (AggregateException)
            {
                _logger.LogInformation("Task canceled");
            }

            _logger.LogInformation("Consumer disposed");
            _cancellationSource.Dispose();

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _consumer.Dispose();
        }
    }
}