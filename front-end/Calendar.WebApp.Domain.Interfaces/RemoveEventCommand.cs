using System;
using MediatR;

namespace Calendar.WebApp.Domain.Interfaces
{
    public record RemoveEventCommand(Guid EventId) : IRequest;
}