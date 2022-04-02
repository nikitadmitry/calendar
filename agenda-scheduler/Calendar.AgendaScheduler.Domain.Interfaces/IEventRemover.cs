using System;
using System.Threading;
using System.Threading.Tasks;

namespace Calendar.AgendaScheduler.Domain.Interfaces
{
    public interface IEventRemover
    {
        Task RemoveAsync(Guid eventId, CancellationToken cancellationToken = default);
    }
}