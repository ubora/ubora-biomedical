using System;

namespace Ubora.Domain.Projects.StructuredInformations.Events
{
    public interface IDeviceStructuredInformationEvent
    {
        Guid DeviceStructuredInformationId { get; }
    }
}