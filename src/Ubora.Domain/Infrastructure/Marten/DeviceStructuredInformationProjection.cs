using Marten;
using Ubora.Domain.Projects.StructuredInformations;
using Ubora.Domain.Projects.StructuredInformations.Events;

namespace Ubora.Domain.Infrastructure.Marten
{
    internal class DeviceStructuredInformationProjection<TEvent> : AggregateMemberProjectionBase<DeviceStructuredInformation, TEvent>
        where TEvent : class, IDeviceStructuredInformationEvent
    {
        protected override DeviceStructuredInformation LoadAggregate(IDocumentSession session, TEvent @event)
        {
            return session.Load<DeviceStructuredInformation>(@event.DeviceStructuredInformationId) ?? new DeviceStructuredInformation();
        }
    }
}