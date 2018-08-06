using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Members.Events;
using Ubora.Domain.Projects._Specifications;
using Ubora.Domain.Users;

namespace Ubora.Domain.Projects.Members.Commands
{
    public class AcceptRequestToJoinProjectCommand : UserCommand
    {
        public Guid RequestId { get; set; }

        internal class Handler : ICommandHandler<AcceptRequestToJoinProjectCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(AcceptRequestToJoinProjectCommand command)
            {
                var request = _documentSession.LoadOrThrow<RequestToJoinProject>(command.RequestId);
                var userProfile = _documentSession.LoadOrThrow<Users.UserProfile>(request.AskingToJoinMemberId);
                var project = _documentSession.LoadOrThrow<Project>(request.ProjectId);

                var isUserAlreadyMember = project.DoesSatisfy(new HasMember(request.AskingToJoinMemberId));
                if (isUserAlreadyMember)
                {
                    return CommandResult.Failed($"[{userProfile.FullName}] is already member of project [{project.Title}].");
                }

                request.Accept();

                var @event = new MemberAcceptedToJoinProjectEvent(
                    initiatedBy: command.Actor,
                    projectId: request.ProjectId,
                    userId: request.AskingToJoinMemberId
                );

                _documentSession.Events.Append(request.ProjectId, @event);
                _documentSession.Store(request);
                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
