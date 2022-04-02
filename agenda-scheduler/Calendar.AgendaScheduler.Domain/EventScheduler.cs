using System;
using System.Threading;
using System.Threading.Tasks;
using Calendar.Agenda.Domain.Entities;
using Calendar.AgendaScheduler.DataAccess.Interfaces;
using Calendar.AgendaScheduler.Domain.Interfaces;

public class EventScheduler : IEventScheduler
{
    private readonly IEventsRepository _eventsRepository;
    private readonly IEventValidator _eventValidator;

    public EventScheduler(IEventsRepository eventsRepository, IEventValidator eventValidator)
    {
        _eventsRepository = eventsRepository;
        _eventValidator = eventValidator;
    }

    public async Task ScheduleAsync(Event @event, CancellationToken cancellationToken = default)
    {
        var validationResult = await _eventValidator.IsValidAsync(@event, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new InvalidOperationException(validationResult.Message);
        }

        await _eventsRepository.AddAsync(@event, cancellationToken);
    }
}