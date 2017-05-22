using System;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Projects.DeviceClassification
{
    public class SaveDeviceClassificationToProjectCommand : UserCommand
    {
        public Guid ProjectId { get; set; }
        public string DeviceClassification { get; set; }
        public Guid Id { get; set; }
    }
}
