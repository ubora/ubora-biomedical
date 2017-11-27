using System;
using System.Collections.Generic;
using System.Text;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Projects.StructuredInformations.Commands
{
    public class EditDeviceStructuredInformationOnUserAndEnvironmentCommand : UserProjectCommand
    {
        public DeviceStructuredInformation.UserAndEnvironmentInformation UserAndEnvironmentInformation { get; set; }
    }
}
