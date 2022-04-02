using System.Threading;
using System.Threading.Tasks;
using Calendar.Agenda.Domain.Entities.Messages;
using Calendar.AgendaViewer.DataAccess.Contracts;
using Calendar.Kafka.Abstractions;

namespace Calendar.AgendaViewer.Domain
{
    public class EventAddedMessageHandler : IMessageHandler<EventAddedMessage>
    {
        private readonly IEventsRepository _eventsRepository;

        public EventAddedMessageHandler(IEventsRepository eventsRepository)
        {
            _eventsRepository = eventsRepository;
        }

        public Task HandleAsync(EventAddedMessage message, CancellationToken cancellationToken)
        {
            return _eventsRepository.AddAsync(message, cancellationToken);
        }
    }
}