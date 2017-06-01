using Marten;
using System;
using System.Linq;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Users;

namespace Ubora.Domain.Projects.Members
{
    public class RemoveMemberFromProjectCommandHandler : ICommandHandler<RemoveMemberFromProjectCommand>
    {
        private readonly IDocumentSession _documentSession;

        public RemoveMemberFromProjectCommandHandler(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public ICommandResult Handle(RemoveMemberFromProjectCommand cmd)
        {
            var userProfile = _documentSession.Load<UserProfile>(cmd.UserId);
            if (userProfile == null) throw new InvalidOperationException();

            var project = _documentSession.Load<Project>(cmd.ProjectId);

            var isUserMember = project.Members.Any(m => m.UserId == cmd.UserId);
            if (!isUserMember)
            {
                return new CommandResult($"User [{cmd.UserId}] is not part of project [{cmd.ProjectId}]");
            }

            var isProjectLeader = project.Members.Any(m => m.UserId == cmd.UserId && m is ProjectLeader);

            if (isProjectLeader)
            {
                return new CommandResult($"User [{cmd.UserId}] can not be removed from project [{cmd.ProjectId}] because user is project leader");
            }

            var @event = new MemberRemovedFromProjectEvent(cmd.Actor)
            {
                ProjectId = cmd.ProjectId,
                UserFullName = userProfile.FullName,
                UserId = cmd.UserId
            };

            _documentSession.Events.Append(cmd.ProjectId, @event);
            _documentSession.SaveChanges();

            return new CommandResult();
        }
    }
}
