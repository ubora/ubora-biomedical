using System;
using Marten;
using Marten.Events;
using Ubora.Domain.Commands;
using Ubora.Domain.Events;
using Ubora.Domain.Projects.Events;

namespace Ubora.Domain.Projects
{
    public class CreateProjectCommand : ICommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public UserInfo UserInfo { get; set; }
    }

    public class CreateProjectCommandHandler : ICommandHandler<CreateProjectCommand>
    {
        private readonly IDocumentSession _documentSession;

        public CreateProjectCommandHandler(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public void Handle(CreateProjectCommand command)
        {
            _documentSession.Events.Append(Guid.NewGuid(), new ProjectCreated(command.Name, command.UserInfo));
            _documentSession.SaveChanges();
        }
    }
}