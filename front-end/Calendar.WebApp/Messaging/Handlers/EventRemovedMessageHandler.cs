using System;
using Calendar.Agenda.Domain.Entities.Messages;
using Microsoft.AspNetCore.SignalR;

namespace Calendar.WebApp.Messaging.Handlers
{
    public class EventRemovedMessageHandler : KafkaToHubMessageHandler<EventRemovedMessage, Guid>
    {
        public EventRemovedMessageHandler(IHubContext<AgendaUpdatesHub> hubContext) : base(hubContext)
        {
        }

        protected override string HubMethod => Constants.Messages.EventRemoved;

        protected override Guid Map(EventRemovedMessage message) => message.EventId;
    }
}