using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Calendar.Agenda.Domain.Entities;

namespace Calendar.AgendaViewer.DataAccess.Contracts
{
    public interface IEventsRepository
    {
        Task AddAsync(Event @event, CancellationToken cancellationToken);
        IAsyncEnumerable<Event> GetAllAsync(CancellationToken cancellationToken);
    }
}