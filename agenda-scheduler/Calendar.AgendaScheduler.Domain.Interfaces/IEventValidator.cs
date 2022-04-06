using System.Threading;
using System.Threading.Tasks;
using Calendar.Agenda.Domain.Entities;
using Calendar.AgendaScheduler.Domain.Entities;

namespace Calendar.AgendaScheduler.Domain.Interfaces
{
    public interface IEventValidator
    {
        Task<ValidationResult> IsValidAsync(Event @event, CancellationToken cancellationToken);
    }
}