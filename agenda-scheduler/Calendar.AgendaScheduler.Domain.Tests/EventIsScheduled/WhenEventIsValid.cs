using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Calendar.Agenda.Domain.Entities;
using Calendar.AgendaScheduler.Domain.Tests.Fixtures;
using Xunit;

namespace Calendar.AgendaScheduler.Domain.Tests.EventIsScheduled
{
    public class WhenEventIsValid : EventSchedulerFixture
    {
        public WhenEventIsValid()
        {
            EventIsValid();
        }

        [Theory, AutoData]
        public async Task EventIsPersisted(Event @event)
        {
            await Subject.ScheduleAsync(@event);

            AssertEventIsPersisted(@event);
        }

        [Theory, AutoData]
        public async Task MessageIsPublished(Event @event)
        {
            await Subject.ScheduleAsync(@event);

            AssertMessageIsPublished(@event);
        }
    }
}