using Calendar.Agenda.Domain.Entities;
using MediatR;

namespace Calendar.WebApp.Domain.Interfaces
{
    public record EventsQuery : IRequest<Event[]>;
}