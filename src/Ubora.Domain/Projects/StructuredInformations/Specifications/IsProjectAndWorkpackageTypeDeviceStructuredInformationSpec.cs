using System;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;

namespace Ubora.Domain.Projects.StructuredInformations.Specifications
{
    public class IsProjectAndWorkpackageTypeDeviceStructuredInformationSpec : Specification<DeviceStructuredInformation>
    {
        public Guid ProjectId { get; }
        public WorkpackageType WorkpackageType { get; }

        public IsProjectAndWorkpackageTypeDeviceStructuredInformationSpec(Guid projectId, WorkpackageType workpackageType)
        {
            ProjectId = projectId;
            WorkpackageType = workpackageType;
        }
        
        internal override Expression<Func<DeviceStructuredInformation, bool>> ToExpression()
        {
            return deviceStructuredInformation => deviceStructuredInformation.ProjectId == ProjectId 
                                                  && deviceStructuredInformation.WorkpackageType == WorkpackageType;
        }
    }
}