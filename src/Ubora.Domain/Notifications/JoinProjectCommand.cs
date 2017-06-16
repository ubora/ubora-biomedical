﻿using System;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Users;
using Ubora.Domain.Projects;
using System.Linq;
using Ubora.Domain.Projects.Members;

namespace Ubora.Domain.Notifications
{
    public class JoinProjectCommand : UserProjectCommand
    {
        public Guid AskingToJoin { get; set; }
    }

    internal class JoinProjectCommandHandler : CommandHandler<JoinProjectCommand>
    {
        public JoinProjectCommandHandler(IDocumentSession documentSession) : base(documentSession)
        {
        }

        public override ICommandResult Handle(JoinProjectCommand cmd)
        {
            var userProfile = DocumentSession.Load<UserProfile>(cmd.AskingToJoin);
            if (userProfile == null) throw new InvalidOperationException();

            var project = DocumentSession.Load<Project>(cmd.ProjectId);

            var isUserAlreadyMember = project.DoesSatisfy(new HasMember(cmd.AskingToJoin));
            if (isUserAlreadyMember)
            {
                return new CommandResult($"[{cmd.AskingToJoin}] is already member of project [{cmd.ProjectId}].");
            }

            var projectLeaderId = project.Members
                .Where(x => x is ProjectLeader)
                .Select(x => x.UserId)
                .First();

            var joinProject = new InvitationToProject(Guid.NewGuid(), projectLeaderId, cmd.AskingToJoin, cmd.ProjectId);

            DocumentSession.Store(joinProject);
            DocumentSession.SaveChanges();

            return new CommandResult();
        }
    }
}
