using System.Threading;
using System.Threading.Tasks;
using Calendar.Agenda.Domain.Entities.Messages;

namespace Calendar.AgendaScheduler.DataAccess.Interfaces
{
    public interface IMessagePublisher
    {
        Task PublishAsync<T>(T message, CancellationToken cancellationToken) where T : IMessage;
    }
}