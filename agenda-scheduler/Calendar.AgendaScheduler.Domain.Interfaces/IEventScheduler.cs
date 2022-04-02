using System.Threading;
using System.Threading.Tasks;
using Calendar.Agenda.Domain.Entities;

namespace Calendar.AgendaScheduler.Domain.Interfaces
{
    public interface IEventScheduler
    {
        Task ScheduleAsync(Event @event, CancellationToken cancellationToken = default);
    }
}