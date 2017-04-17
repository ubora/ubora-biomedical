using System;
using Marten;
using Ubora.Domain.Events;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Events;

namespace Ubora.Domain.Projects
{
    public class CommandResult : ICommandResult
    {
        public bool IsSuccess { get; set; }
    }

    public class CreateProject : ICommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public UserInfo UserInfo { get; set; }
    }

    public class CreateProjectHandler : ICommandHandler<CreateProject>
    {
        private readonly IDocumentSession _documentSession;

        public CreateProjectHandler(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public ICommandResult Handle(CreateProject command)
        {
            _documentSession.Events.Append(command.Id, new ProjectCreated(command.Name, command.UserInfo));
            _documentSession.SaveChanges();

            return new CommandResult();
        }
    }
}