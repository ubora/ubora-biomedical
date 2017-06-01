using Autofac;
using System;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Projects;

namespace Ubora.Web.Authorization
{
    public interface IIsMemberPartOfProject
    {
        bool Satisfy(Guid projectId, Guid userId);
    }

    public class IsMemberPartOfProject : IIsMemberPartOfProject
    {
        public bool Satisfy(Guid projectId, Guid userId)
        {
            using (var scope = Startup.Container.BeginLifetimeScope())
            {
                var processor = scope.Resolve<IQueryProcessor>();
                var project = processor.FindById<Project>(projectId);
                var isMember = project.DoesSatisfy(new HasMember(userId));

                return isMember;
            }
        }
    }
}
