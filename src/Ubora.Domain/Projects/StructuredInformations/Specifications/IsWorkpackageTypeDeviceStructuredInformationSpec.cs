using System;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Projects.StructuredInformations.Specifications
{
    public class IsWorkpackageTypeDeviceStructuredInformationSpec : Specification<DeviceStructuredInformation>
    {
        public DeviceStructuredInformationWorkpackageTypes WorkpackageType { get; }

        public IsWorkpackageTypeDeviceStructuredInformationSpec(DeviceStructuredInformationWorkpackageTypes workpackageType)
        {
            WorkpackageType = workpackageType;
        }
        
        internal override Expression<Func<DeviceStructuredInformation, bool>> ToExpression()
        {
            return deviceStructuredInformation => deviceStructuredInformation.WorkpackageType == WorkpackageType;
        }
    }
}