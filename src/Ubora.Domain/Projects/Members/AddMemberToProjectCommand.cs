using System;
using Ubora.Domain.Infrastructure.Commands;
using Marten;
using Ubora.Domain.Users;

namespace Ubora.Domain.Projects.Members
{
    public class AddMemberToProjectCommand : UserProjectCommand
    {
        public Guid UserId { get; set; }
    }

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

            var isUserAlreadyMember = project.DoesSatisfy(new HasMember(cmd.UserId));
            if (isUserAlreadyMember)
            {
                return new CommandResult($"[{userProfile.FullName}] is already member of project [{project.Title}].");
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
