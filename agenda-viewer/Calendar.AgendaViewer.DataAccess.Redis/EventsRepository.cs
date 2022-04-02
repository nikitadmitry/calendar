using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Calendar.Agenda.Domain.Entities;
using Calendar.AgendaViewer.DataAccess.Contracts;
using StackExchange.Redis;

namespace Calendar.AgendaViewer.DataAccess.Memory
{
    public class EventsRepository : IEventsRepository
    {
        private const string SetName = "calendar:agenda:events";
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public EventsRepository(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }

        public Task AddAsync(Event @event, CancellationToken cancellationToken)
        {
            var db = _connectionMultiplexer.GetDatabase();

            return db.HashSetAsync(new RedisKey(SetName),
                @event.Id.ToString(),
                JsonSerializer.Serialize(@event),
                flags: CommandFlags.FireAndForget);
        }

        public Task RemoveAsync(Guid eventId, CancellationToken cancellationToken)
        {
            var db = _connectionMultiplexer.GetDatabase();
            return db.HashDeleteAsync(new RedisKey(SetName), eventId.ToString(), CommandFlags.FireAndForget);
        }

        public async IAsyncEnumerable<Event> GetAllAsync([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var db = _connectionMultiplexer.GetDatabase();

            var hashEntries = await db.HashGetAllAsync(new RedisKey(SetName));

            foreach (var row in hashEntries)
            {
                yield return JsonSerializer.Deserialize<Event>(row.Value)!;
            }
        }
    }
}