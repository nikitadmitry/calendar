using Calendar.Agenda.Domain.Entities;
using Calendar.Agenda.Domain.Entities.Messages;
using Microsoft.AspNetCore.SignalR;

namespace Calendar.WebApp.Messaging.Handlers
{
    public class EventAddedMessageHandler : KafkaToHubMessageHandler<EventAddedMessage, Event>
    {
        public EventAddedMessageHandler(IHubContext<AgendaUpdatesHub> hubContext) : base(hubContext)
        {
        }

        protected override string HubMethod => Constants.Messages.EventAdded;
        protected override Event Map(EventAddedMessage message) => message;
    }
}