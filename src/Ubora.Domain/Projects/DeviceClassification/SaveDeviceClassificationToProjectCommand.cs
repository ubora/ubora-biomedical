using System;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Projects.DeviceClassification
{
    public class SetDeviceClassificationForProjectCommand : UserProjectCommand
    {
        public string DeviceClassification { get; set; }
    }
}
