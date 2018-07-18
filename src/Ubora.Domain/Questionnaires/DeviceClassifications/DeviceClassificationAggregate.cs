using System;
using Newtonsoft.Json;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects;
using Ubora.Domain.Questionnaires.DeviceClassifications.Events;

namespace Ubora.Domain.Questionnaires.DeviceClassifications
{
    public class DeviceClassificationAggregate : Entity<DeviceClassificationAggregate>, IProjectEntity
    {
        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }
        public DateTime StartedAt { get; private set; }
        public DateTime? FinishedAt { get; private set; }
        public DeviceClassificationQuestionnaireTree QuestionnaireTree { get; private set; }
        public int QuestionnaireTreeVersion { get; private set; }

        [JsonIgnore]
        public bool IsFinished => FinishedAt.HasValue;
        
        [JsonIgnore]
        public virtual bool IsStopped => FinishedAt.HasValue && QuestionnaireTree.FindNextUnansweredQuestion() != null;

        private void Apply(DeviceClassificationStartedEvent e)
        {
            if (e.Id == default(Guid)) { throw new InvalidOperationException(); }
            if (e.ProjectId == default(Guid)) { throw new InvalidOperationException(); }
            if (QuestionnaireTree != null) { throw new InvalidOperationException(); }

            Id = e.Id;
            ProjectId = e.ProjectId;
            StartedAt = e.StartedAt;
            QuestionnaireTree = e.QuestionnaireTree;
            QuestionnaireTreeVersion = e.QuestionnaireTreeVersion;
        }

        private void Apply(DeviceClassificationAnsweredEvent e)
        {
            var question = QuestionnaireTree.FindNextUnansweredQuestion();
            if (question.Id != e.QuestionId)
            {
                throw new InvalidOperationException();
            }

            question.ChooseAnswer(e.AnswerId, e.AnsweredAt);

            if (QuestionnaireTree.FindNextUnansweredQuestion() == null)
            {
                FinishedAt = e.AnsweredAt;
            }
        }

        private void Apply(DeviceClassificationStoppedEvent e)
        {
            if (IsFinished)
            {
                throw new InvalidOperationException();
            }
            FinishedAt = e.StoppedAt;
        }
    }
}
