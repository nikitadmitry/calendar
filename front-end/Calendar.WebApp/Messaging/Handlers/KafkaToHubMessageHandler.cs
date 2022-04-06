using System.Threading;
using System.Threading.Tasks;
using Calendar.Agenda.Domain.Entities.Messages;
using Calendar.Kafka.Abstractions;
using Microsoft.AspNetCore.SignalR;

namespace Calendar.WebApp.Messaging.Handlers
{
    public abstract class KafkaToHubMessageHandler<TKafkaMessage, THubMessage> : IMessageHandler<TKafkaMessage> where TKafkaMessage : IMessage
    {
        private readonly IHubContext<AgendaUpdatesHub> _hubContext;

        protected KafkaToHubMessageHandler(IHubContext<AgendaUpdatesHub> hubContext)
        {
            _hubContext = hubContext;
        }

        protected abstract string HubMethod { get; }

        protected abstract THubMessage Map(TKafkaMessage message);

        public Task HandleAsync(TKafkaMessage message, CancellationToken cancellationToken)
        {
            return _hubContext.Clients.All.SendAsync(HubMethod, Map(message), cancellationToken);
        }
    }
}