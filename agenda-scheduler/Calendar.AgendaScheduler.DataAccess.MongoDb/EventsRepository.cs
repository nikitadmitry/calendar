using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Calendar.Agenda.Domain.Entities;
using Calendar.AgendaScheduler.DataAccess.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Calendar.AgendaScheduler.DataAccess.MongoDb
{
    public class EventsRepository : IEventsRepository, IInitializable
    {
        private const string CollectionName = "events";
        private readonly IMongoDatabase _database;

        public EventsRepository(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task<IList<Event>> GetAsync(Expression<Func<Event, bool>> expression, CancellationToken cancellationToken)
        {
            var cursor = await Collection.FindAsync(new ExpressionFilterDefinition<Event>(expression),
                cancellationToken: cancellationToken);
            return await cursor.ToListAsync(cancellationToken);
        }

        public async Task<IList<Event>> GetAllAsync(CancellationToken cancellationToken)
        {
            var cursor = await Collection.FindAsync(Builders<Event>.Filter.Empty, cancellationToken: cancellationToken);
            return await cursor.ToListAsync(cancellationToken);
        }

        public Task AddAsync(Event @event, CancellationToken cancellationToken)
        {
            return Collection.InsertOneAsync(@event, new InsertOneOptions(), cancellationToken);
        }

        public Task RemoveAsync(Guid eventId, CancellationToken cancellationToken)
        {
            return Collection.DeleteOneAsync(new ExpressionFilterDefinition<Event>(x => x.Id == eventId), cancellationToken);
        }

        private IMongoCollection<Event> Collection => _database.GetCollection<Event>(CollectionName);


        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            var collectionExists = await (await _database.ListCollectionNamesAsync(new ListCollectionNamesOptions
            {
                Filter = new BsonDocument("name", CollectionName)
            }, cancellationToken)).AnyAsync(cancellationToken);

            if (!collectionExists)
            {
                await _database.CreateCollectionAsync(CollectionName, cancellationToken: cancellationToken);
            }

            await CreateIndexes(cancellationToken, nameof(Event.Start), nameof(Event.End));
        }

        private Task CreateIndexes(CancellationToken cancellationToken, params string[] fieldNames)
        {
            var createModels = fieldNames
                .Select(fieldName => new StringFieldDefinition<Event>(fieldName))
                .Select(field => new IndexKeysDefinitionBuilder<Event>().Ascending(field))
                .Select(definition => new CreateIndexModel<Event>(definition));

            return Collection.Indexes.CreateManyAsync(createModels, cancellationToken);
        }
    }
}