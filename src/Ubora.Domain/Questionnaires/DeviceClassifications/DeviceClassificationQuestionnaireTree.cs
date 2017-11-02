using System;
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

            var conditionList = conditions.ToList();

            foreach (var q in questions)
            {
                foreach (var a in q.Answers)
                {
                    if (a.GetDeviceClass() != null)
                    {
                        conditionList.Add(new ChosenAnswerDeviceClassCondition(q.Id, a.Id, a.GetDeviceClass()));
                    }
                }
            }

            Conditions = conditionList.ToArray();
        }

        [JsonConstructor]
        protected DeviceClassificationQuestionnaireTree()
        {
        }

        protected virtual void ValidateConditions(ChosenAnswerDeviceClassCondition[] conditions)
        {
            if (conditions == null) throw new ArgumentNullException(nameof(conditions));

            // Validate 
            foreach (var entry in conditions.SelectMany(r => r.QuestionIdsWithExpectedChosenAnswerIds))
            {
                var question = FindQuestionOrThrow(entry.Key);

                var answer = question.Answers.SingleOrDefault(a => a.Id == entry.Value);
                if (answer == null)
                {
                    throw new InvalidOperationException($"Answer not found with ID: {entry.Value} from Question with ID: {entry.Key}");
                }
            }
        }
    }
}
