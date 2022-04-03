using System;
using System.Threading;
using System.Threading.Tasks;
using Calendar.Agenda.Domain.Entities;
using Calendar.AgendaScheduler.Domain.Tests.Fixtures;
using FluentAssertions;
using Xunit;

namespace Calendar.AgendaScheduler.Domain.Tests.EventIsValidated
{
    public class WhenEventHasNoName : EventValidatorFixture
    {
        [Fact]
        public async Task ValidationFails()
        {
            var @event = new Event
            {
                Start = new DateTime(2022, 4, 1, 10, 0, 0),
                End = new DateTime(2022, 4, 1, 11, 0, 0)
            };

            var validationResult = await Subject.IsValidAsync(@event, CancellationToken.None);

            validationResult.IsValid.Should().BeFalse();
        }
    }
}