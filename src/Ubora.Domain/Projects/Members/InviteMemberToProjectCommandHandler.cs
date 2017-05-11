using Marten;
using System;
using System.Linq;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Users;

namespace Ubora.Domain.Projects.Members
{
    public class InviteMemberToProjectCommandHandler : ICommandHandler<InviteMemberToProjectCommand>
    {
        private readonly IDocumentSession _documentSession;

        public InviteMemberToProjectCommandHandler(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public ICommandResult Handle(InviteMemberToProjectCommand command)
        {
            var userProfile = _documentSession.Load<UserProfile>(command.UserId);
            if (userProfile == null) throw new InvalidOperationException();

            var project = _documentSession.Load<Project>(command.ProjectId);

            var isUserAlreadyMember = project.Members.Any(m => m.UserId == command.UserId);
            if (isUserAlreadyMember)
            {
                return new CommandResult(false);
            }

            var @event = new MemberInvitedToProjectEvent(command.UserInfo)
            {
                ProjectId = command.ProjectId,
                UserId = command.UserId,
                UserFullName = userProfile.FullName
            };

            _documentSession.Events.Append(command.ProjectId, @event);
            _documentSession.SaveChanges();

            return new CommandResult(true);
        }
    }
}
