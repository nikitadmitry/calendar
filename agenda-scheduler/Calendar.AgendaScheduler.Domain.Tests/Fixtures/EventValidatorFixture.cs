using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using Calendar.Agenda.Domain.Entities;
using Calendar.AgendaScheduler.DataAccess.Interfaces;
using Moq;

namespace Calendar.AgendaScheduler.Domain.Tests.Fixtures
{
    public class EventValidatorFixture : BaseFixture<EventValidator>
    {
        private readonly IList<Event> _events = new List<Event>();

        protected EventValidatorFixture()
        {
            Mocker.Setup((IEventsRepository x) => x.GetAsync(
                    It.IsAny<Expression<Func<Event, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Expression<Func<Event, bool>> expression, CancellationToken _) =>
                    _events.Where(expression.Compile()).ToList());
        }

        protected void EventExists(Event @event) => _events.Add(@event);
    }
}