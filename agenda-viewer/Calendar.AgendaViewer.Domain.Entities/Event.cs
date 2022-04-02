using System;

namespace Calendar.AgendaViewer.Domain.Entities
{
    public class Event
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}