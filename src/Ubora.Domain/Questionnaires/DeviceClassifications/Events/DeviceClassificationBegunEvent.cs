using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Questionnaires.DeviceClassifications.Events
{
    public class DeviceClassificationBegunEvent : ProjectEvent
    {
        public DeviceClassificationBegunEvent(UserInfo initiatedBy, Guid projectId, Guid id, DateTime begunAt, DeviceClassificationQuestionnaireTree questionnaireTree) : base(initiatedBy, projectId)
        {
            Id = id;
            BegunAt = begunAt;
            QuestionnaireTree = questionnaireTree;
        }

        public Guid Id { get; private set; }
        public DateTime BegunAt { get; private set; }
        public DeviceClassificationQuestionnaireTree QuestionnaireTree { get; private set; }

        public override string GetDescription()
        {
            throw new NotImplementedException();
        }
    }
}
