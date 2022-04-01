using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace Calendar.AgendaViewer.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventController : ControllerBase
    {
        [HttpGet("list")]
        public IEnumerable<Event> GetAllEvents(CancellationToken cancellationToken)
        {
            return Enumerable.Range(1, 5).Select(index => new Event
                {
                    Start = new DateTime(2022, 4, 3 * index),
                    End = new DateTime(2022, 4, 3 * index + 3),
                    Name = $"Event #{index}"
                })
                .ToArray();
        }
    }
}