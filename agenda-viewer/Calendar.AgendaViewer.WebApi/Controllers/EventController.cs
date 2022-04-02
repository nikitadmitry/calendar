using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Calendar.Agenda.Domain.Entities;
using Calendar.AgendaViewer.DataAccess.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Calendar.AgendaViewer.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly IEventsRepository _eventsRepository;

        public EventController(IEventsRepository eventsRepository)
        {
            _eventsRepository = eventsRepository;
        }

        [HttpGet("all")]
        public IAsyncEnumerable<Event> GetAllEventsAsync(CancellationToken cancellationToken)
        {
            return _eventsRepository.GetAllAsync(cancellationToken);
        }

#if DEBUG
        [HttpPost("add")]
        public Task AddEventAsync(Event @event, CancellationToken cancellationToken)
        {
            return _eventsRepository.AddAsync(@event, cancellationToken);
        }
#endif
    }
}