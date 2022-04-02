using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Calendar.Agenda.Domain.Entities;

namespace Calendar.AgendaScheduler.DataAccess.Interfaces
{
    public interface IEventsRepository
    {
        Task<IList<Event>> GetAsync(Expression<Func<Event, bool>> expression, CancellationToken cancellationToken);
        Task<IList<Event>> GetAllAsync(CancellationToken cancellationToken);
        Task AddAsync(Event @event, CancellationToken cancellationToken);
        Task RemoveAsync(Guid eventId, CancellationToken cancellationToken);
    }
}