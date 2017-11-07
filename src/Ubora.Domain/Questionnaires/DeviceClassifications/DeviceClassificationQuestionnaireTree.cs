using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Ubora.Domain.Questionnaires.DeviceClassifications
{
    public class DeviceClassificationQuestionnaireTree : QuestionnaireTreeBase<Question, Answer>
    {
        [JsonProperty(nameof(Conditions))]
        private ChosenAnswerDeviceClassCondition[] _conditions;

        [JsonIgnore]
        public ChosenAnswerDeviceClassCondition[] Conditions
        {
            get { return _conditions; }
            protected set
            {
                if (_conditions != null) throw new InvalidOperationException("Let's keep this immutable.");
                ValidateConditions(value);
                _conditions = value;
            }
        }

        public DeviceClassificationQuestionnaireTree(Question[] questions, ChosenAnswerDeviceClassCondition[] conditions)
        {
            if (conditions == null) throw new ArgumentNullException(nameof(conditions));
            Questions = questions ?? throw new ArgumentNullException(nameof(questions));

            Conditions = conditions.ToArray();
        }

        [JsonConstructor]
        protected DeviceClassificationQuestionnaireTree()
        {
        }

        public IEnumerable<DeviceClass> GetDeviceClassHits()
        {
            return Conditions.Where(x => x.IsFulfilled(this)).Select(x => x.DeviceClass);
        }

        protected virtual void ValidateConditions(ChosenAnswerDeviceClassCondition[] conditions)
        {
            if (conditions == null) throw new ArgumentNullException(nameof(conditions));

            // Validate questions and answers exist
            foreach (var entry in conditions.SelectMany(r => r.QuestionIdsWithExpectedChosenAnswerIds))
            {
                var question = FindQuestionOrThrow(entry.Key);

                var answer = question.Answers.SingleOrDefault(a => a.Id == entry.Value);
                if (answer == null)
                {
                    throw new InvalidOperationException($"Answer not found with ID: {entry.Value} from Question with ID: {entry.Key}");
                }
            }

            // Validate no duplicate ID-s
            var duplicateConditionIds = conditions.GroupBy(q => q.Id).Where(g => g.Count() > 1).Select(g => g.Key);
            if (duplicateConditionIds.Any())
            {
                throw new InvalidOperationException($"Following conditon ID-s are duplicated: {string.Join(",", duplicateConditionIds)}");
            }
        }
    }
}
