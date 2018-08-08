using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Members.Events;
using Ubora.Domain.Projects._Specifications;
using Ubora.Domain.Users;

namespace Ubora.Domain.Projects.Members.Commands
{
    public class AddMemberToProjectCommand : UserProjectCommand
    {
        public Guid UserId { get; set; }

        internal class Handler : CommandHandler<AddMemberToProjectCommand>
        {
            public Handler(IDocumentSession documentSession) : base(documentSession)
            {
            }

            public override ICommandResult Handle(AddMemberToProjectCommand cmd)
            {
                var userProfile = DocumentSession.LoadOrThrow<UserProfile>(cmd.UserId);
                var project = DocumentSession.LoadOrThrow<Project>(cmd.ProjectId);

                var isUserAlreadyMember = project.DoesSatisfy(new HasMember(cmd.UserId));
                if (isUserAlreadyMember)
                {
                    return CommandResult.Failed($"[{cmd.UserId}] is already member of project [{cmd.ProjectId}].");
                }

                var @event = new MemberAddedToProjectEvent(
                    initiatedBy: cmd.Actor,
                    projectId: cmd.ProjectId,
                    userId: cmd.UserId
                );

                DocumentSession.Events.Append(cmd.ProjectId, @event);
                DocumentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
