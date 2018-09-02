using System;
using Marten;
using System.Collections.Generic;
using System.Linq;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects.Workpackages;
using Ubora.Domain.Projects.Workpackages.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects._Commands
{
    public interface IMarkdownToQuillDeltaConverter
    {
        QuillDelta Convert(string markdown);
    }

    public class ConvertProjectWpStepsFromMarkdownToQuillDeltaCommand : UserProjectCommand
    {
        public class Handler : ICommandHandler<ConvertProjectWpStepsFromMarkdownToQuillDeltaCommand>
        {
            private readonly IDocumentSession _documentSession;
            private readonly IMarkdownToQuillDeltaConverter _markdownToQuillDeltaConverter;

            public Handler(IDocumentSession documentSession, IMarkdownToQuillDeltaConverter markdownToQuillDeltaConverter)
            {
                _documentSession = documentSession;
                _markdownToQuillDeltaConverter = markdownToQuillDeltaConverter;
            }

            public ICommandResult Handle(ConvertProjectWpStepsFromMarkdownToQuillDeltaCommand cmd)
            {
                var project = _documentSession.LoadOrThrow<Project>(cmd.ProjectId);

                if (project.HasMarkdownBeenConvertedToQuillDelta)
                {
                    throw new InvalidOperationException("Already converted.");
                }

                var wp1 = _documentSession.Load<WorkpackageOne>(cmd.ProjectId);
                var wp2 = _documentSession.Load<WorkpackageTwo>(cmd.ProjectId);
                var wp3 = _documentSession.Load<WorkpackageThree>(cmd.ProjectId);
                var wp4 = _documentSession.Load<WorkpackageFour>(cmd.ProjectId);

                var events = new List<object>();

                var projectDescriptionQuillDelta = _markdownToQuillDeltaConverter.Convert(project.Description);
                events.Add(new ProjectDescriptionEditedEventV2(cmd.Actor, project.Id, projectDescriptionQuillDelta));

                if (wp1 != null)
                {
                    var wp1Events = 
                        wp1.Steps.Select(step => 
                            {
                                var quillDelta = _markdownToQuillDeltaConverter.Convert(step.Content);
                                return new WorkpackageOneStepEditedEventV2(cmd.Actor, project.Id, step.Id, step.Title, quillDelta);
                            });

                    events.AddRange(wp1Events);
                }

                if (wp2 != null)
                {
                    var wp2Events =
                        wp2.Steps.Select(step =>
                            {
                                var quillDelta = _markdownToQuillDeltaConverter.Convert(step.Content);
                                return new WorkpackageTwoStepEditedEventV2(cmd.Actor, project.Id, step.Id, step.Title, quillDelta);
                            });

                    events.AddRange(wp2Events);
                }

                if (wp3 != null)
                {
                    var wp3Events =
                        wp3.Steps.Select(step =>
                        {
                            var quillDelta = _markdownToQuillDeltaConverter.Convert(step.Content);
                            return new WorkpackageThreeStepEditedEventV2(cmd.Actor, project.Id, step.Id, step.Title, quillDelta);
                        });

                    events.AddRange(wp3Events);
                }

                if (wp4 != null)
                {
                    var wp4Events =
                        wp4.Steps.Select(step =>
                        {
                            var quillDelta = _markdownToQuillDeltaConverter.Convert(step.Content);
                            return new WorkpackageFourStepEditedEventV2(cmd.Actor, project.Id, step.Id, step.Title, quillDelta);
                        });

                    events.AddRange(wp4Events);
                }

                if (events.Any())
                {
                    _documentSession.Events.Append(project.Id, events.ToArray());
                    _documentSession.SaveChanges();
                }

                return CommandResult.Success;
            }
        }
    }
}
