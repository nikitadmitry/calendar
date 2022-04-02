using System.Threading;
using System.Threading.Tasks;

namespace Calendar.AgendaScheduler.DataAccess.Interfaces
{
    public interface IInitializable
    {
        Task InitializeAsync(CancellationToken cancellationToken);
    }
}