using System;
using System.Linq;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects._Specifications;
using Ubora.Domain.Users;

namespace Ubora.Domain.Projects.Members.Commands
{
    public class JoinProjectCommand : UserProjectCommand
    {
        internal class Handler : CommandHandler<JoinProjectCommand>
        {
            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(JoinProjectCommand cmd)
            {
                var userProfile = DocumentSession.LoadOrThrow<UserProfile>(cmd.Actor.UserId);
                var project = DocumentSession.LoadOrThrow<Project>(cmd.ProjectId);

                var isUserAlreadyMember = project.DoesSatisfy(new HasMember(cmd.Actor.UserId));
                if (isUserAlreadyMember)
                {
                    return new CommandResult($"[{userProfile.FullName}] is already member of project [{project.Title}].");
                }

                var projectLeaderId = project.Members
                    .Where(x => x is ProjectLeader)
                    .Select(x => x.UserId)
                    .First();

                var joinProject = new RequestToJoinProject(projectLeaderId, cmd.Actor.UserId, cmd.ProjectId);

                DocumentSession.Store(joinProject);
                DocumentSession.SaveChanges();

                return new CommandResult();
            }
        }
    }
}
