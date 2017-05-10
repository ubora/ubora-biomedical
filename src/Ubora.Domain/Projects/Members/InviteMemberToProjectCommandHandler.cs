using Marten;
using System;
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
            var project = _documentSession.Load<Project>(command.ProjectId);

            if (project == null)
            {
                throw new InvalidOperationException();
            }

            var userProfile = _documentSession.Load<UserProfile>(command.UserId);

            if (userProfile == null)
            {
                throw new InvalidOperationException();
            }

            var @event = new MemberInvitedToProjectEvent(command.UserInfo)
            {
                ProjectId = command.ProjectId,
                UserId = command.UserId
            };

            _documentSession.Events.Append(command.ProjectId, @event);
            _documentSession.SaveChanges();

            return new CommandResult(true);
        }
    }
}
