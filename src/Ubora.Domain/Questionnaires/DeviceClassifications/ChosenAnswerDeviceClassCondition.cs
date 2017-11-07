using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Ubora.Domain.Questionnaires.DeviceClassifications
{
    public class ChosenAnswerDeviceClassCondition : IDeviceClassCondition
    {
        public string Id { get; private set; } // Id is for debugging and validation purpose.
        public DeviceClass DeviceClass { get; protected set; }
        public Dictionary<string, string> QuestionIdsWithExpectedChosenAnswerIds { get; protected set; } // Not the greatest name.

        public ChosenAnswerDeviceClassCondition(string questionId, string answerId, DeviceClass deviceClass)
            : this($"{questionId}+{answerId}", new Dictionary<string, string> { { questionId, answerId} }, deviceClass)
        {
        }

        public ChosenAnswerDeviceClassCondition(string ruleId, Dictionary<string, string> questionIdsWithExpectedChosenAnswerIds, DeviceClass deviceClass)
        {
            QuestionIdsWithExpectedChosenAnswerIds = questionIdsWithExpectedChosenAnswerIds ?? throw new ArgumentNullException(nameof(questionIdsWithExpectedChosenAnswerIds));
            DeviceClass = deviceClass ?? throw new ArgumentNullException(nameof(deviceClass));
            Id = ruleId ?? throw new ArgumentNullException(nameof(ruleId));
        }

        [JsonConstructor]
        protected ChosenAnswerDeviceClassCondition()
        {
        }

        public bool IsFulfilled(DeviceClassificationQuestionnaireTree questionnaireTree)
        {
            var isFromGivenQuestionnaire = questionnaireTree.Conditions.Any(x => x.Id == this.Id);
            if (!isFromGivenQuestionnaire)
            {
                throw new InvalidOperationException("Condition not from questionnaire.");
            }

            foreach (var entry in QuestionIdsWithExpectedChosenAnswerIds)
            {
                var question = questionnaireTree.FindQuestionOrThrow(entry.Key);
                var answer = question.Answers.First(a => a.Id == entry.Value);

                if (answer.IsChosen != true)
                {
                    return false;
                }
            }

            return true;
        }
    }
}