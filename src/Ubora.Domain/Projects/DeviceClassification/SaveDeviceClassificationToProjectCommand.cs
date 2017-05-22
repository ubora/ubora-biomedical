using System;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Projects.DeviceClassification
{
    public class SaveDeviceClassificationToProjectCommand : UserProjectCommand
    {
        public string DeviceClassification { get; set; }
        public Guid Id { get; set; }
    }
}
