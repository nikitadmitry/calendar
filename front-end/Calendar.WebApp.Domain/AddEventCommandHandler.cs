using System.Threading;
using System.Threading.Tasks;
using Calendar.AgendaScheduler.Client;
using Calendar.WebApp.Domain.Interfaces;
using MediatR;

namespace Calendar.WebApp.Domain
{
    public class AddEventCommandHandler : IRequestHandler<AddEventCommand>
    {
        private readonly IAgendaSchedulerClient _agendaSchedulerClient;

        public AddEventCommandHandler(IAgendaSchedulerClient agendaSchedulerClient)
        {
            _agendaSchedulerClient = agendaSchedulerClient;
        }

        public async Task<Unit> Handle(AddEventCommand request, CancellationToken cancellationToken)
        {
            await _agendaSchedulerClient.AddEventAsync(request.Event, cancellationToken);
            return Unit.Value;
        }
    }
}