using Marten;
using System;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Members.Events;
using Ubora.Domain.Users;

namespace Ubora.Domain.Notifications.Invitation
{
    public class AcceptInvitationToProjectCommand : UserCommand
    {
        public Guid InvitationId { get; set; }

        internal class Handler : ICommandHandler<AcceptInvitationToProjectCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(AcceptInvitationToProjectCommand command)
            {
                var invite = _documentSession.LoadOrThrow<InvitationToProject>(command.InvitationId);
                var userProfile = _documentSession.LoadOrThrow<UserProfile>(invite.InvitedMemberId);
                var project = _documentSession.LoadOrThrow<Project>(invite.ProjectId);

                var isUserAlreadyMember = project.DoesSatisfy(new HasMember(invite.InvitedMemberId));
                if (isUserAlreadyMember)
                {
                    return new CommandResult($"[{invite.InvitedMemberId}] is already member of project [{invite.ProjectId}].");
                }

                invite.Accept();

                var @event = new MemberAddedToProjectEvent(command.Actor)
                {
                    ProjectId = invite.ProjectId,
                    UserId = invite.InvitedMemberId,
                    UserFullName = userProfile.FullName
                };

                _documentSession.Events.Append(invite.ProjectId, @event);
                _documentSession.Store(invite);
                _documentSession.SaveChanges();

                return new CommandResult();
            }
        }
    }
}