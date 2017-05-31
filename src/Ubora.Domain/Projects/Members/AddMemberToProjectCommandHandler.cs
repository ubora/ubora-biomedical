using Marten;
using System;
using System.Linq;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Users;

namespace Ubora.Domain.Projects.Members
{
    internal class AddMemberToProjectCommandHandler : CommandHandler<AddMemberToProjectCommand>
    {
        public AddMemberToProjectCommandHandler(IDocumentSession documentSession) : base(documentSession)
        {
        }

        public override ICommandResult Handle(AddMemberToProjectCommand cmd)
        {
            var userProfile = DocumentSession.Load<UserProfile>(cmd.UserId);
            if (userProfile == null) throw new InvalidOperationException();

            var project = DocumentSession.Load<Project>(cmd.ProjectId);

            var isUserAlreadyMember = project.Members.Any(m => m.UserId == cmd.UserId);
            if (isUserAlreadyMember)
            {
                return new CommandResult($"[{cmd.UserId}] is already member of project [{cmd.ProjectId}].");
            }

            var @event = new MemberAddedToProjectEvent(cmd.Actor)
            {
                ProjectId = cmd.ProjectId,
                UserId = cmd.UserId,
                UserFullName = userProfile.FullName
            };

            DocumentSession.Events.Append(cmd.ProjectId, @event);
            DocumentSession.SaveChanges();

            return new CommandResult();
        }
    }
}
