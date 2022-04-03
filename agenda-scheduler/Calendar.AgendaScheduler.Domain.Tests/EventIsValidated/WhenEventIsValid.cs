using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Calendar.Agenda.Domain.Entities;
using Calendar.AgendaScheduler.Domain.Tests.Fixtures;
using FluentAssertions;
using Xunit;

namespace Calendar.AgendaScheduler.Domain.Tests.EventIsValidated
{
    public class WhenEventIsValid : EventValidatorFixture
    {
        public WhenEventIsValid()
        {
            EventExists(new Event
            {
                Start = new DateTime(2022, 4, 1, 10, 0, 0),
                End = new DateTime(2022, 4, 1, 11, 0, 0)
            });

            EventExists(new Event
            {
                Start = new DateTime(2022, 4, 1, 12, 0, 0),
                End = new DateTime(2022, 4, 1, 13, 0, 0)
            });
        }

        [Fact]
        public async Task ValidationSucceeds()
        {
            var @event = new Event
            {
                Name = "event",
                Start = new DateTime(2022, 4, 1, 11, 0, 0),
                End = new DateTime(2022, 4, 1, 12, 0, 0)
            };

            var validationResult = await Subject.IsValidAsync(@event, CancellationToken.None);

            validationResult.IsValid.Should().BeTrue();
        }
    }
}