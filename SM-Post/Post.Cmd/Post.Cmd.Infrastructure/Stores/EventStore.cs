using System.Diagnostics;
using CQRS.Core.Domain;
using CQRS.Core.Events;
using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructures;
using CQRS.Core.Producers;
using Post.Cmd.Domain;

namespace Post.Cmd.Infrastructure.Stores
{
    public class EventStore : IEventStore
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IEventProducer _eventProducer;
        public EventStore(IEventStoreRepository eventStoreRepository, IEventProducer eventProducer)
        {
            _eventProducer = eventProducer;
            _eventStoreRepository = eventStoreRepository;
        }

        public async Task<List<BaseEvent>> GetEventAsync(Guid aggregateId)
        {
            var eventStore = await _eventStoreRepository.FindByAggregateIdAsync(aggregateId);
            if (eventStore is null || !eventStore.Any())
            {
                throw new AggregateNotFoundException("Incorrect post ID provided!");
            }

            return eventStore.OrderBy(x => x.Version).Select(x => x.EventData).ToList();
        }

        public async Task SaveEventAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion)
        {
            var eventStore = await _eventStoreRepository.FindByAggregateIdAsync(aggregateId);
            // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/
            // Unary expresion  
            if (expectedVersion != -1 && eventStore[^1].Version != expectedVersion)
            {
                throw new ConcurrencyException();
            }

            var version = expectedVersion;
            foreach (var @event in events)
            {
                version++;
                @event.Version = version;
                var eventType = @event.GetType().Name;
                var eventModel = new EventModel
                {
                    TimeStamp = DateTime.Now,
                    AggregateIdentifier = aggregateId,
                    AggregateType = nameof(PostAggregate),
                    Version = version,
                    EventType = eventType,
                    EventData = @event
                };

                await _eventStoreRepository.SaveAsync(eventModel);

                var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC");
                await _eventProducer.ProduceAsync(topic, @event);
            }
        }
    }
}