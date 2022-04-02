using System.Threading;
using System.Threading.Tasks;
using Calendar.Agenda.Domain.Entities;
using Refit;

namespace Calendar.AgendaViewer.Client
{
    public interface IAgendaViewerClient
    {
        [Get("/api/event/all")]
        Task<Event[]> GetAllEventsAsync(CancellationToken cancellationToken = default);
    }
}