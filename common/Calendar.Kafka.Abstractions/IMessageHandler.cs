using System.Threading;
using System.Threading.Tasks;
using Calendar.Agenda.Domain.Entities.Messages;

namespace Calendar.Kafka.Abstractions
{
    public interface IMessageHandler<in T> where T : IMessage
    {
        Task HandleAsync(T message, CancellationToken cancellationToken);
    }
}