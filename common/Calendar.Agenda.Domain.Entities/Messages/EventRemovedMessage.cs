using System;

namespace Calendar.Agenda.Domain.Entities.Messages
{
    public class EventRemovedMessage : IMessage
    {
        public Guid EventId { get; set; }
    }
}