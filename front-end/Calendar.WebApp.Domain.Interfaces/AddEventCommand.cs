using Calendar.Agenda.Domain.Entities;
using MediatR;

namespace Calendar.WebApp.Domain.Interfaces
{
    public record AddEventCommand(Event Event) : IRequest<Event>;
}