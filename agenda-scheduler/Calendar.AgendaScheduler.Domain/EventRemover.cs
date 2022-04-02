using System;
using System.Threading;
using System.Threading.Tasks;
using Calendar.Agenda.Domain.Entities.Messages;
using Calendar.AgendaScheduler.DataAccess.Interfaces;
using Calendar.AgendaScheduler.Domain.Interfaces;

namespace Calendar.AgendaScheduler.Domain
{
    public class EventRemover : IEventRemover
    {
        private readonly IEventsRepository _eventsRepository;
        private readonly IMessagePublisher _messagePublisher;

        public EventRemover(IEventsRepository eventsRepository,
            IMessagePublisher messagePublisher)
        {
            _eventsRepository = eventsRepository;
            _messagePublisher = messagePublisher;
        }

        public async Task RemoveAsync(Guid eventId, CancellationToken cancellationToken = default)
        {
            await _eventsRepository.RemoveAsync(eventId, cancellationToken);

            await _messagePublisher.PublishAsync(new EventRemovedMessage
            {
                EventId = eventId
            }, cancellationToken);
        }
    }
}