using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Calendar.Agenda.Domain.Entities;
using Calendar.Agenda.Domain.Entities.Messages;
using Calendar.AgendaScheduler.DataAccess.Interfaces;
using Calendar.AgendaScheduler.Domain.Interfaces;

namespace Calendar.AgendaScheduler.Domain
{
    public class EventScheduler : IEventScheduler
    {
        private readonly IEventsRepository _eventsRepository;
        private readonly IEventValidator _eventValidator;
        private readonly IMessagePublisher _messagePublisher;
        private readonly IMapper _mapper;

        public EventScheduler(IEventsRepository eventsRepository,
            IEventValidator eventValidator,
            IMessagePublisher messagePublisher,
            IMapper mapper)
        {
            _eventsRepository = eventsRepository;
            _eventValidator = eventValidator;
            _messagePublisher = messagePublisher;
            _mapper = mapper;
        }

        public async Task ScheduleAsync(Event @event, CancellationToken cancellationToken = default)
        {
            var validationResult = await _eventValidator.IsValidAsync(@event, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Message);
            }

            await PublishMessageAsync(@event, cancellationToken);

            await _eventsRepository.AddAsync(@event, cancellationToken);
        }

        private Task PublishMessageAsync(Event @event, CancellationToken cancellationToken)
        {
            var message = _mapper.Map<EventAddedMessage>(@event);
            return _messagePublisher.PublishAsync(message, cancellationToken);
        }
    }
}