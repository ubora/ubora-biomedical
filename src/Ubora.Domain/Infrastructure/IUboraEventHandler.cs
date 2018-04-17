using Marten.Events;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Infrastructure
{
    public interface IUboraEventHandler<TEvent> where TEvent : UboraEvent
    {
        void Handle(IEvent eventWithMetadata);
    }
}