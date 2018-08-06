using System;
using System.Linq;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Notifications.Specifications;
using Ubora.Domain.Projects._Specifications;
using Ubora.Domain.Users;

namespace Ubora.Domain.Projects.Members.Commands
{
    public class InviteProjectMentorCommand : UserProjectCommand
    {
        public Guid UserId { get; set; }

        internal class Handler : ICommandHandler<InviteProjectMentorCommand>
        {
            private readonly IDocumentSession _documentSession;
            private readonly IQueryProcessor _queryProcessor;

            public Handler(IDocumentSession documentSession, IQueryProcessor queryProcessor)
            {
                _documentSession = documentSession;
                _queryProcessor = queryProcessor;
            }

            public ICommandResult Handle(InviteProjectMentorCommand cmd)
            {
                var project = _documentSession.LoadOrThrow<Project>(cmd.ProjectId);
                var userProfile = _documentSession.LoadOrThrow<Users.UserProfile>(cmd.UserId);

                var isAlreadyMentor = project.DoesSatisfy(new HasMember<ProjectMentor>(cmd.UserId));
                if (isAlreadyMentor)
                {
                    return CommandResult.Failed("User is already a mentor of this project.");
                }

                var isAlreadyInvited = _queryProcessor.Find(new IsFromProjectSpec<ProjectMentorInvitation> { ProjectId = project.Id }
                                                            && new HasPendingNotifications<ProjectMentorInvitation>(userProfile.UserId)).Any();
                if (isAlreadyInvited)
                {
                    return CommandResult.Failed($"{userProfile.FullName} already has a pending mentor invitation to this project.");
                }

                var invitation = new ProjectMentorInvitation(userProfile.UserId, project.Id, cmd.Actor.UserId);

                _documentSession.Store(invitation);
                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
