using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Members.Events;
using Ubora.Domain.Projects._Specifications;
using Ubora.Domain.Users;

namespace Ubora.Domain.Projects.Members.Commands
{
    public class RemoveMemberFromProjectCommand : UserProjectCommand
    {
        public Guid UserId { get; set; }

        public class Handler : ICommandHandler<RemoveMemberFromProjectCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(RemoveMemberFromProjectCommand cmd)
            {
                var userProfile = _documentSession.LoadOrThrow<UserProfile>(cmd.UserId);
                var project = _documentSession.LoadOrThrow<Project>(cmd.ProjectId);

                var isUserMember = project.DoesSatisfy(new HasMember(cmd.UserId));
                if (!isUserMember)
                {
                    return CommandResult.Failed($"User [{cmd.UserId}] is not part of project [{cmd.ProjectId}]");
                }

                var isProjectLeader = project.DoesSatisfy(new HasLeader(cmd.UserId));
                if (isProjectLeader)
                {
                    return CommandResult.Failed($"User [{cmd.UserId}] can not be removed from project [{cmd.ProjectId}] because user is project leader");
                }

                var @event = new MemberRemovedFromProjectEvent(
                    initiatedBy: cmd.Actor,
                    projectId: cmd.ProjectId,
                    userId: cmd.UserId
                );

                _documentSession.Events.Append(cmd.ProjectId, @event);
                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
