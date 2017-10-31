using System;
using System.Collections.Generic;
using System.Text;
using Marten;
using Marten.Events;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Projects._Commands
{
    public class DeleteProjectCommand : UserProjectCommand
    {

       
        internal class Handler : ICommandHandler<DeleteProjectCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }


            public ICommandResult Handle(DeleteProjectCommand cmd)
            {
                var project = _documentSession.LoadOrThrow<Project>(cmd.ProjectId);

                _documentSession.Delete<Project>(project.Id);
                _documentSession.DeleteWhere<IEvent>(e => e.StreamId == cmd.ProjectId);

                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
