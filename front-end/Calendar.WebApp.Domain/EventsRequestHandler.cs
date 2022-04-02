using System.Threading;
using System.Threading.Tasks;
using Calendar.Agenda.Domain.Entities;
using Calendar.AgendaViewer.Client;
using Calendar.WebApp.Domain.Interfaces;
using MediatR;

namespace Calendar.WebApp.Domain
{
    public class EventsRequestHandler : IRequestHandler<EventsQuery, Event[]>
    {
        private readonly IAgendaViewerClient _agendaViewerClient;

        public EventsRequestHandler(IAgendaViewerClient agendaViewerClient)
        {
            _agendaViewerClient = agendaViewerClient;
        }

        public Task<Event[]> Handle(EventsQuery request, CancellationToken cancellationToken)
        {
            return _agendaViewerClient.GetAllEventsAsync(cancellationToken);
        }
    }
}