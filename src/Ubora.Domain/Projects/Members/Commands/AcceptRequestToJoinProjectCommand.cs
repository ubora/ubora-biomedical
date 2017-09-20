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
                var userProfile = _documentSession.LoadOrThrow<UserProfile>(request.AskingToJoinMemberId);
                var project = _documentSession.LoadOrThrow<Project>(request.ProjectId);

                var isUserAlreadyMember = project.DoesSatisfy(new HasMember(request.AskingToJoinMemberId));
                if (isUserAlreadyMember)
                {
                    return new CommandResult($"[{userProfile.FullName}] is already member of project [{project.Title}].");
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
}
