using System;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Questionnaires.DeviceClassifications.Events;

namespace Ubora.Domain.Questionnaires.DeviceClassifications
{
    public class DeviceClassificationAggregate : Entity<DeviceClassificationAggregate>
    {
        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }
        public DateTime BegunAt { get; private set; }
        public DateTime FinishedAt { get; private set; }
        public DeviceClassificationQuestionnaireTree QuestionnaireTree { get; private set; }

        private void Apply(DeviceClassificationBegunEvent e)
        {
            if (e.Id == default(Guid)) { throw new InvalidOperationException(); }
            if (e.ProjectId == default(Guid)) { throw new InvalidOperationException(); }
            if (QuestionnaireTree != null) { throw new InvalidOperationException(); }

            Id = e.Id;
            ProjectId = e.ProjectId;
            BegunAt = e.BegunAt;
            QuestionnaireTree = e.QuestionnaireTree;
        }

        private void Apply(DeviceClassificationAnsweredEvent e)
        {
            var question = QuestionnaireTree.FindNextUnansweredQuestion();
            if (question.Id != e.QuestionId)
            {
                throw new InvalidOperationException();
            }

            question.ChooseAnswer(e.AnswerId);

            if (QuestionnaireTree.FindNextUnansweredQuestion() == null)
            {
                FinishedAt = e.AnsweredAt;
            }
        }
    }
}
