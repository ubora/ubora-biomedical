using Marten;
using System;
using System.Linq;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Users;

namespace Ubora.Domain.Projects.Members
{
    internal class InviteMemberToProjectCommandHandler : CommandHandler<InviteMemberToProjectCommand>
    {
        public InviteMemberToProjectCommandHandler(IDocumentSession documentSession) : base(documentSession)
        {
        }

        public override ICommandResult Handle(InviteMemberToProjectCommand cmd)
        {
            var userProfile = DocumentSession.Load<UserProfile>(cmd.UserId);
            if (userProfile == null) throw new InvalidOperationException();

            var project = DocumentSession.Load<Project>(cmd.ProjectId);

            var isUserAlreadyMember = project.Members.Any(m => m.UserId == cmd.UserId);
            if (isUserAlreadyMember)
            {
                return new CommandResult(false);
            }

            var @event = new MemberInvitedToProjectEvent(cmd.UserInfo)
            {
                ProjectId = cmd.ProjectId,
                UserId = cmd.UserId,
                UserFullName = userProfile.FullName
            };

            DocumentSession.Events.Append(cmd.ProjectId, @event);
            DocumentSession.SaveChanges();

            return new CommandResult(true);
        }
    }
}
