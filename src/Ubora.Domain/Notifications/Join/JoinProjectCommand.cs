using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Users;
using Ubora.Domain.Projects;
using System.Linq;
using Ubora.Domain.Projects.Members;

namespace Ubora.Domain.Notifications.Join
{
    public class JoinProjectCommand : UserProjectCommand
    {
    }

    internal class JoinProjectCommandHandler : CommandHandler<JoinProjectCommand>
    {
        public JoinProjectCommandHandler(IDocumentSession documentSession) : base(documentSession)
        {
        }

        public override ICommandResult Handle(JoinProjectCommand cmd)
        {
            var userProfile = DocumentSession.Load<UserProfile>(cmd.Actor.UserId);
            if (userProfile == null) throw new InvalidOperationException();

            var project = DocumentSession.Load<Project>(cmd.ProjectId);

            var isUserAlreadyMember = project.DoesSatisfy(new HasMember(cmd.Actor.UserId));
            if (isUserAlreadyMember)
            {
                return new CommandResult($"[{userProfile.FullName}] is already member of project [{project.Title}].");
            }

            var projectLeaderId = project.Members
                .Where(x => x is ProjectLeader)
                .Select(x => x.UserId)
                .First();

            var joinProject = new RequestToJoinProject(Guid.NewGuid(), projectLeaderId, cmd.Actor.UserId, cmd.ProjectId);

            DocumentSession.Store(joinProject);
            DocumentSession.SaveChanges();

            return new CommandResult();
        }
    }
}
