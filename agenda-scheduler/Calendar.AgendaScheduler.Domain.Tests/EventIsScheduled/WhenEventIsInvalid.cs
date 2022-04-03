using System;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Calendar.Agenda.Domain.Entities;
using Calendar.AgendaScheduler.Domain.Tests.Fixtures;
using FluentAssertions;
using Xunit;

namespace Calendar.AgendaScheduler.Domain.Tests.EventIsScheduled
{
    public class WhenEventIsInvalid : EventSchedulerFixture
    {
        public WhenEventIsInvalid()
        {
            EventIsInvalid();
        }

        [Theory, AutoData]
        public async Task Throws(Event @event)
        {
            await Subject.Invoking(x => x.ScheduleAsync(@event))
                .Should().ThrowAsync<InvalidOperationException>();
        }

        [Theory, AutoData]
        public async Task EventIsNotPersisted(Event @event)
        {
            try
            {
                await Subject.ScheduleAsync(@event);
                true.Should().BeFalse("Should throw");
            }
            catch (Exception e)
            {
                // ignored
            }

            AssertEventIsNotPersisted(@event);
        }

        [Theory, AutoData]
        public async Task MessageIsNotPublished(Event @event)
        {
            try
            {
                await Subject.ScheduleAsync(@event);
                true.Should().BeFalse("Should throw");
            }
            catch (Exception e)
            {
                // ignored
            }

            AssertMessageIsNotPublished(@event);
        }
    }
}