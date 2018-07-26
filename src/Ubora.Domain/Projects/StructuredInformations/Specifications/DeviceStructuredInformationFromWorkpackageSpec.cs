using System;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Projects.StructuredInformations.Specifications
{
    public class DeviceStructuredInformationFromWorkpackageSpec : Specification<DeviceStructuredInformation>
    {
        public DeviceStructuredInformationWorkpackageTypes WorkpackageType { get; }

        public DeviceStructuredInformationFromWorkpackageSpec(DeviceStructuredInformationWorkpackageTypes workpackageType)
        {
            WorkpackageType = workpackageType;
        }
        
        internal override Expression<Func<DeviceStructuredInformation, bool>> ToExpression()
        {
            return deviceStructuredInformation => deviceStructuredInformation.WorkpackageType == WorkpackageType;
        }
    }
}