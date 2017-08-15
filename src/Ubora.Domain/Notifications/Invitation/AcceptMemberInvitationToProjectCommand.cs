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
    }

    internal class AcceptInvitationToProjectCommandHandler : ICommandHandler<AcceptInvitationToProjectCommand>
    {
        private readonly IDocumentSession _documentSession;

        public AcceptInvitationToProjectCommandHandler(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public ICommandResult Handle(AcceptInvitationToProjectCommand command)
        {
            var invite = _documentSession.Load<InvitationToProject>(command.InvitationId);
            if (invite == null) throw new InvalidOperationException();

            var userProfile = _documentSession.Load<UserProfile>(invite.InvitedMemberId);
            if (userProfile == null) throw new InvalidOperationException();

            var project = _documentSession.Load<Project>(invite.ProjectId);

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
