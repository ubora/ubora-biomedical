using Marten.Events;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Infrastructure
{
    public interface IEventHandler<TEvent> where TEvent : UboraEvent
    {
        void Handle(IEvent eventWithMetadata);
    }
}
