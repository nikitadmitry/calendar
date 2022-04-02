using System.Threading;
using System.Threading.Tasks;
using Calendar.AgendaScheduler.Client;
using Calendar.WebApp.Domain.Interfaces;
using MediatR;

namespace Calendar.WebApp.Domain
{
    public class RemoveEventCommandHandler : IRequestHandler<RemoveEventCommand>
    {
        private readonly IAgendaSchedulerClient _agendaSchedulerClient;

        public RemoveEventCommandHandler(IAgendaSchedulerClient agendaSchedulerClient)
        {
            _agendaSchedulerClient = agendaSchedulerClient;
        }

        public async Task<Unit> Handle(RemoveEventCommand request, CancellationToken cancellationToken)
        {
            await _agendaSchedulerClient.RemoveEventAsync(request.EventId, cancellationToken);

            return Unit.Value;
        }
    }
}