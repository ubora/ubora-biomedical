using System;

namespace Ubora.Domain.Projects.DeviceClassification
{
    public interface IMainQuestion<T>
    {
        Guid Id { get; }
        T NextMainQuestion { get; }
        bool IsLastMainQuestion { get; }
    }
}
