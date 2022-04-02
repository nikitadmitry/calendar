using System;
using Microsoft.Extensions.Configuration;

namespace Calendar.AgendaScheduler.DataAccess.Kafka
{
    public class KafkaTopicsProvider
    {
        private readonly IConfiguration _configuration;

        public KafkaTopicsProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Get<T>()
        {
            return _configuration[$"Kafka:Topics:{typeof(T).Name}"]
                   ?? throw new InvalidOperationException($"Kafka topic not defined for {typeof(T).Name}");
        }
    }
}