using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Members.Events;
using Ubora.Domain.Projects._Specifications;
using Ubora.Domain.Users;

namespace Ubora.Domain.Projects.Members.Commands
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
                    return CommandResult.Failed($"[{userProfile.FullName}] is already member of project [{project.Title}].");
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

                return CommandResult.Success;
            }
        }
    }
}