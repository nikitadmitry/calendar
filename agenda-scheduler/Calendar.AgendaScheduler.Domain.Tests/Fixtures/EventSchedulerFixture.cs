using System.Threading;
using AutoMapper;
using Calendar.Agenda.Domain.Entities;
using Calendar.Agenda.Domain.Entities.Messages;
using Calendar.AgendaScheduler.DataAccess.Interfaces;
using Calendar.AgendaScheduler.Domain.Interfaces;
using Moq;

namespace Calendar.AgendaScheduler.Domain.Tests.Fixtures
{
    public class EventSchedulerFixture : BaseFixture<EventScheduler>
    {
        protected EventSchedulerFixture()
        {
            Mocker.Setup((IMapper x) => x.Map<EventAddedMessage>(It.IsAny<Event>()))
                .Returns<Event>(e => new EventAddedMessage { Id = e.Id });
        }

        protected void EventIsValid()
        {
            Mocker.Setup((IEventValidator x) =>
                    x.IsValidAsync(It.IsAny<Event>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ValidationResult.Succeed);
        }

        protected void EventIsInvalid()
        {
            Mocker.Setup((IEventValidator x) =>
                    x.IsValidAsync(It.IsAny<Event>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(ValidationResult.Failed("Invalid event"));
        }

        protected void AssertEventIsPersisted(Event @event) => AssertEventIsPersisted(@event, Times.Once());

        protected void AssertEventIsNotPersisted(Event @event) => AssertEventIsPersisted(@event, Times.Never());

        protected void AssertMessageIsPublished(Event @event) => AssertMessageIsPublished(@event, Times.Once());

        protected void AssertMessageIsNotPublished(Event @event) => AssertMessageIsPublished(@event, Times.Never());

        private void AssertEventIsPersisted(Event @event, Times times)
        {
            Mocker.Verify<IEventsRepository>(x =>
                x.AddAsync(@event, It.IsAny<CancellationToken>()), times);
        }

        private void AssertMessageIsPublished(Event @event, Times times)
        {
            Mocker.Verify<IMessagePublisher>(x =>
                    x.PublishAsync(It.Is<EventAddedMessage>(m => m.Id == @event.Id), It.IsAny<CancellationToken>()),
                times);
        }
    }
}