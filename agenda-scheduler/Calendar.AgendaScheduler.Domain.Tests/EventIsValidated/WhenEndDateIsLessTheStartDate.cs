using System;
using System.Threading;
using System.Threading.Tasks;
using Calendar.Agenda.Domain.Entities;
using Calendar.AgendaScheduler.Domain.Tests.Fixtures;
using FluentAssertions;
using Xunit;

namespace Calendar.AgendaScheduler.Domain.Tests.EventIsValidated
{
    public class WhenEndDateIsLessTheStartDate : EventValidatorFixture
    {
        [Fact]
        public async Task ValidationFails()
        {
            var @event = new Event
            {
                Name = "event",
                Start = new DateTime(2022, 4, 1, 10, 0, 0)
            };
            @event.End = @event.Start.AddMinutes(-1);

            var validationResult = await Subject.IsValidAsync(@event, CancellationToken.None);

            validationResult.IsValid.Should().BeFalse();
        }
    }
}