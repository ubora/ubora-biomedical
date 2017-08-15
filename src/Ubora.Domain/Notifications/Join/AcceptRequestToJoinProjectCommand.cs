using Marten;
using System;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects;
using Ubora.Domain.Projects.Members;
using Ubora.Domain.Projects.Members.Events;
using Ubora.Domain.Users;

namespace Ubora.Domain.Notifications.Join
{
    public class AcceptRequestToJoinProjectCommand : UserCommand
    {
        public Guid RequestId { get; set; }
    }

    internal class AcceptRequestToJoinProjectCommandHandler : ICommandHandler<AcceptRequestToJoinProjectCommand>
    {
        private readonly IDocumentSession _documentSession;

        public AcceptRequestToJoinProjectCommandHandler(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public ICommandResult Handle(AcceptRequestToJoinProjectCommand command)
        {
            var request = _documentSession.Load<RequestToJoinProject>(command.RequestId);
            if (request == null) throw new InvalidOperationException();

            var userProfile = _documentSession.Load<UserProfile>(request.AskingToJoinMemberId);
            if (userProfile == null) throw new InvalidOperationException();

            var project = _documentSession.Load<Project>(request.ProjectId);

            var isUserAlreadyMember = project.DoesSatisfy(new HasMember(request.AskingToJoinMemberId));
            if (isUserAlreadyMember)
            {
                return new CommandResult($"[{request.AskingToJoinMemberId}] is already member of project [{request.ProjectId}].");
            }

            request.Accept();

            var @event = new MemberAcceptedToJoinProjectEvent(command.Actor)
            {
                ProjectId = request.ProjectId,
                UserId = request.AskingToJoinMemberId,
                UserFullName = userProfile.FullName
            };

            _documentSession.Events.Append(request.ProjectId, @event);
            _documentSession.Store(request);
            _documentSession.SaveChanges();

            return new CommandResult();
        }
    }
}
