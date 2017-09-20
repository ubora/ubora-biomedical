using System;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;
using Ubora.Domain.Projects.Repository;

namespace Ubora.Domain.Projects._Specifications
{
    public class IsProjectFileSpec : Specification<ProjectFile>
    {
        public Guid ProjectId { get;  }

        public IsProjectFileSpec(Guid projectId)
        {
            ProjectId = projectId;
        }

        internal override Expression<Func<ProjectFile, bool>> ToExpression()
        {
            return projectFile => projectFile.ProjectId == ProjectId;
        }
    }
}
