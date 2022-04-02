using System;
using System.Threading;
using System.Threading.Tasks;
using Calendar.Agenda.Domain.Entities;
using Microsoft.AspNetCore.SignalR;

namespace Calendar.WebApp.Messaging
{
    public class AgendaUpdatesHub : Hub
    {
        public Task NotifyEventAddedAsync(Event @event, CancellationToken cancellationToken)
            => Clients.All.SendAsync(Constants.Messages.EventAdded, @event, cancellationToken);

        public Task NotifyEventRemovedAsync(Guid eventId, CancellationToken cancellationToken)
            => Clients.All.SendAsync(Constants.Messages.EventRemoved, eventId, cancellationToken);
    }
}