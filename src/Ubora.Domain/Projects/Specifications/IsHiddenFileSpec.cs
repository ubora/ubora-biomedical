using System;
using System.Linq.Expressions;
using Ubora.Domain.Infrastructure.Specifications;
using Ubora.Domain.Projects.Repository;

namespace Ubora.Domain.Projects.Specifications
{
    public class IsHiddenFileSpec : Specification<ProjectFile>
    {
        internal override Expression<Func<ProjectFile, bool>> ToExpression()
        {
            return projectFile => projectFile.IsHidden;
        }
    }
}
