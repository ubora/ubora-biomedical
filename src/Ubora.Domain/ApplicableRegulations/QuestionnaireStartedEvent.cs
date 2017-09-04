﻿using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.ApplicableRegulations
{
    public class QuestionnaireStartedEvent : UboraEvent
    {
        public QuestionnaireStartedEvent(UserInfo initiatedBy, Guid newQuestionnaireId, Guid projectId) : base(initiatedBy)
        {
            NewQuestionnaireId = newQuestionnaireId;
            ProjectId = projectId;
        }

        public Guid NewQuestionnaireId { get; private set; }
        public Guid ProjectId { get; private set; }

        public override string GetDescription() => "started questionnaire to find out project's applicable regulations (ISO standard)";
    }
}