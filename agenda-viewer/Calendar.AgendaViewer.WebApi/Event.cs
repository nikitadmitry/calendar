using System;

namespace Calendar.AgendaViewer.WebApi
{
    public class Event
    {
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}