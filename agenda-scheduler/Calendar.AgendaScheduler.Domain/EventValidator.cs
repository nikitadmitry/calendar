using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Calendar.Agenda.Domain.Entities;
using Calendar.AgendaScheduler.DataAccess.Interfaces;
using Calendar.AgendaScheduler.Domain.Interfaces;

namespace Calendar.AgendaScheduler.Domain
{
    public class EventValidator : IEventValidator
    {
        private readonly IEventsRepository _eventsRepository;

        public EventValidator(IEventsRepository eventsRepository)
        {
            _eventsRepository = eventsRepository;
        }

        public async Task<ValidationResult> IsValidAsync(Event @event, CancellationToken cancellationToken)
        {
            var overlaps = await _eventsRepository.GetAsync(x =>
                x.Start <= @event.End && x.End >= @event.Start, cancellationToken);

            if (overlaps.Any())
            {
                return ValidationResult.Failed("Event overlaps with existing events");
            }

            return ValidationResult.Succeed();
        }
    }
}