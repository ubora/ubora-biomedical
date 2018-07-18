using System;
using System.Collections.Generic;
using System.Linq;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Resources.Events;

namespace Ubora.Domain.Resources.Commands
{
    public class EditResourcePageCommand : UserCommand
    {
        public Guid ResourcePageId { get; set; }
        public string Title { get; set; }
        public QuillDelta Body { get; set; }
        public int PreviousContentVersion { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public int MenuPriority { get; set; }

        public class Handler : ICommandHandler<EditResourcePageCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }
            
            public ICommandResult Handle(EditResourcePageCommand cmd)
            {
                var resourcePage = _documentSession.LoadOrThrow<ResourcePage>(cmd.ResourcePageId);

                var events = new List<object>();

                if (resourcePage.MenuPriority != cmd.MenuPriority || resourcePage.CategoryId != cmd.ParentCategoryId)
                {
                    events.Add(new ResourcePageMenuPreferencesChangedEvent(cmd.Actor, resourcePage.Id, cmd.MenuPriority, cmd.ParentCategoryId));
                }

                if (resourcePage.Body != cmd.Body)
                {
                    if (resourcePage.BodyVersion != cmd.PreviousContentVersion)
                    {
                        return CommandResult.Failed("The content has been changed while you were editing. Please refresh the page.");
                    }

                    events.Add(new ResourcePageBodyEditedEvent(
                        initiatedBy: cmd.Actor,
                        resourcePageId: resourcePage.Id,
                        body: cmd.Body,
                        previousBodyVersion: cmd.PreviousContentVersion));
                }

                if (resourcePage.Title != cmd.Title)
                {
                    events.Add(new ResourcePageTitleChangedEvent(
                        initiatedBy: cmd.Actor,
                        resourcePageId: resourcePage.Id,
                        title: cmd.Title));
                }

                if (events.Any())
                {
                    _documentSession.Events.Append(cmd.ResourcePageId, events.ToArray());
                    _documentSession.SaveChanges();
                }
                
                return CommandResult.Success;
            }
        }
    }
}