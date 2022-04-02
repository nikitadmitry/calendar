using System.Threading;
using System.Threading.Tasks;
using Calendar.Agenda.Domain.Entities;
using Calendar.AgendaScheduler.Client;
using Calendar.WebApp.Domain.Interfaces;
using MediatR;

namespace Calendar.WebApp.Domain
{
    public class AddEventCommandHandler : IRequestHandler<AddEventCommand, Event>
    {
        private readonly IAgendaSchedulerClient _agendaSchedulerClient;

        public AddEventCommandHandler(IAgendaSchedulerClient agendaSchedulerClient)
        {
            _agendaSchedulerClient = agendaSchedulerClient;
        }

        public Task<Event> Handle(AddEventCommand request, CancellationToken cancellationToken)
        {
            return _agendaSchedulerClient.AddEventAsync(request.Event, cancellationToken);
        }
    }
}