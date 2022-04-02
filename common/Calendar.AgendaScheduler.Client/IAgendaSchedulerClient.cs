using System;
using System.Threading;
using System.Threading.Tasks;
using Calendar.Agenda.Domain.Entities;
using Refit;

namespace Calendar.AgendaScheduler.Client
{
    public interface IAgendaSchedulerClient
    {
        [Post("/api/agenda/event")]
        Task AddEventAsync([Body]Event @event, CancellationToken cancellationToken = default);

        [Delete("/api/agenda/event/{id}")]
        Task RemoveEventAsync(Guid id, CancellationToken cancellationToken = default);
    }
}