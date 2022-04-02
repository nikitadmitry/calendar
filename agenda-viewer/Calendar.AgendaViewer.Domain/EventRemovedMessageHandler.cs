using System.Threading;
using System.Threading.Tasks;
using Calendar.Agenda.Domain.Entities.Messages;
using Calendar.AgendaViewer.DataAccess.Contracts;
using Calendar.Kafka.Abstractions;

namespace Calendar.AgendaViewer.Domain
{
    public class EventRemovedMessageHandler : IMessageHandler<EventRemovedMessage>
    {
        private readonly IEventsRepository _eventsRepository;

        public EventRemovedMessageHandler(IEventsRepository eventsRepository)
        {
            _eventsRepository = eventsRepository;
        }

        public Task HandleAsync(EventRemovedMessage message, CancellationToken cancellationToken)
        {
            return _eventsRepository.RemoveAsync(message.EventId, cancellationToken);
        }
    }
}