using System.Collections.Generic;

namespace Calendar.Kafka.Configuration.Extensions
{
    public class KafkaConsumerOptions
    {
        public string BrokerList { get; set; }
        public string ConsumerGroupId { get; set; }
        public IDictionary<string, string> Topics { get; set; }
    }
}