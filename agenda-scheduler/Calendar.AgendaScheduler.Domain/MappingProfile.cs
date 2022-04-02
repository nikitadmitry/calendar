using AutoMapper;
using Calendar.Agenda.Domain.Entities;
using Calendar.Agenda.Domain.Entities.Messages;

namespace Calendar.AgendaScheduler.Domain
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Event, EventAddedMessage>();
            CreateMap<Event, EventRemovedMessage>();
        }
    }
}