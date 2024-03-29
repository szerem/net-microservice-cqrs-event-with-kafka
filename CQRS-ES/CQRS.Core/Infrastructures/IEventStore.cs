using CQRS.Core.Events;

namespace CQRS.Core.Infrastructures
{
    public interface IEventStore
    {
        Task SaveEventAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion);
        Task<List<BaseEvent>> GetEventAsync(Guid aggregateId);
    }
}