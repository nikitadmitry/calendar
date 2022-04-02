using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Calendar.Agenda.Domain.Entities;
using Calendar.AgendaScheduler.DataAccess.Interfaces;
using Calendar.AgendaScheduler.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Calendar.AgendaScheduler.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AgendaController : ControllerBase
    {
        private readonly IEventScheduler _eventScheduler;
        private readonly IEventRemover _eventRemover;
        private readonly IEventsRepository _eventsRepository;

        public AgendaController(IEventScheduler eventScheduler,
            IEventRemover eventRemover,
            IEventsRepository eventsRepository)
        {
            _eventScheduler = eventScheduler;
            _eventRemover = eventRemover;
            _eventsRepository = eventsRepository;
        }

        [HttpPost("event")]
        public Task AddEventAsync(Event @event, CancellationToken cancellationToken)
        {
            return _eventScheduler.ScheduleAsync(@event, cancellationToken);
        }

        [HttpDelete("event/{id}")]
        public Task DeleteEventAsync(Guid id, CancellationToken cancellationToken)
        {
            return _eventRemover.RemoveAsync(id, cancellationToken);
        }

#if DEBUG
        [HttpGet("event/all")]
        public Task<IList<Event>> GetAllEventsAsync(CancellationToken cancellationToken)
        {
            return _eventsRepository.GetAllAsync(cancellationToken);
        }

        [HttpGet("event/between/{start}/{end}")]
        public Task<IList<Event>> GetOverlapsAsync(DateTime start, DateTime end, CancellationToken cancellationToken)
        {
            return _eventsRepository.GetAsync(x => x.Start <= end && x.End >= start, cancellationToken);
        }
#endif
    }
}