using Marten;
using System;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects._Specifications;
using Ubora.Domain.Projects.Members.Events;
using Ubora.Domain.Users;

namespace Ubora.Domain.Projects.Members.Commands
{
    public class PromoteProjectLeaderCommand : UserProjectCommand
    {
        public Guid UserId { get; set; }

        public class Handler : ICommandHandler<PromoteProjectLeaderCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(PromoteProjectLeaderCommand cmd)
            {
                var userProfile = _documentSession.LoadOrThrow<Users.UserProfile>(cmd.UserId);
                var project = _documentSession.LoadOrThrow<Project>(cmd.ProjectId);

                var isUserMember = project.DoesSatisfy(new HasMember(cmd.UserId));
                if (!isUserMember)
                {
                    return CommandResult.Failed($"User [{cmd.UserId}] is not part of project [{cmd.ProjectId}]");
                }

                var isProjectLeader = project.DoesSatisfy(new HasLeader(cmd.UserId));
                if (isProjectLeader)
                {
                    return CommandResult.Failed($"Already user [{cmd.UserId}] is project leader in [{cmd.ProjectId}]");
                }

                var @event = new ProjectLeaderPromotedEvent(
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
