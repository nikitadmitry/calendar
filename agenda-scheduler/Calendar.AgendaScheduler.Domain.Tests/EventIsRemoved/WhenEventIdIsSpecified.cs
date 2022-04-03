using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Calendar.Agenda.Domain.Entities.Messages;
using Calendar.AgendaScheduler.DataAccess.Interfaces;
using Calendar.AgendaScheduler.Domain.Tests.Fixtures;
using Moq;
using Xunit;

namespace Calendar.AgendaScheduler.Domain.Tests.EventIsRemoved
{
    public class WhenEventIdIsSpecified : EventRemoverFixture
    {
        [Theory, AutoData]
        public async Task MessageIsPublished(Guid eventId)
        {
            await Subject.RemoveAsync(eventId, CancellationToken.None);

            Mocker.Verify<IMessagePublisher>(x =>
                x.PublishAsync(It.Is<EventRemovedMessage>(m => m.EventId == eventId),
                    It.IsAny<CancellationToken>()));
        }

        [Theory, AutoData]
        public async Task EntityIsRemovedFromStorage(Guid eventId)
        {
            await Subject.RemoveAsync(eventId, CancellationToken.None);

            Mocker.Verify<IEventsRepository>(x =>
                x.RemoveAsync(eventId, It.IsAny<CancellationToken>()));
        }
    }
}