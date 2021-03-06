﻿using System;
using System.Linq;
using Marten;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Projects;
using Ubora.Domain.Questionnaires.DeviceClassifications.Events;

namespace Ubora.Domain.Questionnaires.DeviceClassifications.Commands
{
    public class AnswerDeviceClassificationCommand : UserProjectCommand
    {
        public Guid QuestionnaireId { get; set; }
        public string QuestionId { get; set; }
        public string AnswerId { get; set; }

        internal class Handler : ICommandHandler<AnswerDeviceClassificationCommand>
        {
            private readonly IDocumentSession _documentSession;

            public Handler(IDocumentSession documentSession)
            {
                _documentSession = documentSession;
            }

            public ICommandResult Handle(AnswerDeviceClassificationCommand cmd)
            {
                var questionnaireAggregate = _documentSession.LoadOrThrow<DeviceClassificationAggregate>(cmd.QuestionnaireId);

                var question = questionnaireAggregate.QuestionnaireTree.FindQuestionOrThrow(cmd.QuestionId);
                if (question.IsAnswered)
                {
                    return CommandResult.Failed("Question already answered.");
                }

                var @event = new DeviceClassificationAnsweredEvent(
                    initiatedBy: cmd.Actor,
                    projectId: cmd.ProjectId,
                    questionnaireId: cmd.QuestionnaireId, 
                    questionId: cmd.QuestionId, 
                    answerId: cmd.AnswerId,
                    answeredAt: DateTime.UtcNow);

                _documentSession.Events.Append(questionnaireAggregate.Id, @event);
                _documentSession.SaveChanges();

                return CommandResult.Success;
            }
        }
    }
}
