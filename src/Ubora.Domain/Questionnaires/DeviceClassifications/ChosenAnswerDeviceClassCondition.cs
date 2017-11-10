using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Ubora.Domain.Questionnaires.DeviceClassifications
{
    public class ChosenAnswerDeviceClassCondition
    {
        [JsonProperty("qaIds")]
        public Dictionary<string, string> QuestionIdsWithExpectedChosenAnswerIds { get; protected set; } // Not the greatest name.

        public ChosenAnswerDeviceClassCondition(Dictionary<string, string> questionIdsWithExpectedChosenAnswerIds)
        {
            QuestionIdsWithExpectedChosenAnswerIds = questionIdsWithExpectedChosenAnswerIds ?? throw new ArgumentNullException(nameof(questionIdsWithExpectedChosenAnswerIds));
        }

        public ChosenAnswerDeviceClassCondition(string questionId, string answerId)
            : this(new Dictionary<string, string> { { questionId, answerId} })
        {
        }

        [JsonConstructor]
        protected ChosenAnswerDeviceClassCondition()
        {
        }

        public bool IsFulfilled(DeviceClassificationQuestionnaireTree questionnaireTree)
        {
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

        public void Validate(DeviceClassificationQuestionnaireTree questionnaireTree)
        {
            foreach (var entry in QuestionIdsWithExpectedChosenAnswerIds)
            {
                var question = questionnaireTree.FindQuestionOrThrow(entry.Key);

                var answer = question.Answers.SingleOrDefault(a => a.Id == entry.Value);
                if (answer == null)
                {
                    throw new InvalidOperationException($"Answer not found with ID: {entry.Value} from Question with ID: {entry.Key}");
                }
            }
        }
    }
}