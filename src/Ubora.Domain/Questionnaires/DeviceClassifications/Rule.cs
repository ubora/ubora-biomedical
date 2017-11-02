using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Ubora.Domain.Questionnaires.DeviceClassifications
{
    public class DeviceClassResult
    {
        public IEnumerable<DeviceClass> Hits { get; private set; }

        public DeviceClassResult(IEnumerable<DeviceClass> hits)
        {
            
        }
    }

    public interface IDeviceClassCondition
    {
        bool IsFulfilled(DeviceClassificationQuestionnaireTree questionnaireTree);
    }

    public class ChosenAnswerDeviceClassCondition : IDeviceClassCondition
    {
        public DeviceClass DeviceClass { get; protected set; }
        public Dictionary<string, string> QuestionIdsWithExpectedChosenAnswerIds { get; protected set; } // Not the greatest name.

        public ChosenAnswerDeviceClassCondition(string questionId, string answerId, DeviceClass deviceClass)
            : this("", new Dictionary<string, string> { { questionId, answerId} }, deviceClass)
        {
        }

        public ChosenAnswerDeviceClassCondition(string ruleId, Dictionary<string, string> questionIdsWithExpectedChosenAnswerIds, DeviceClass deviceClass)
        {
            QuestionIdsWithExpectedChosenAnswerIds = questionIdsWithExpectedChosenAnswerIds ?? throw new ArgumentNullException(nameof(questionIdsWithExpectedChosenAnswerIds));
            DeviceClass = deviceClass ?? throw new ArgumentNullException(nameof(deviceClass));
            //Id = ruleId ?? throw new ArgumentNullException(nameof(ruleId));
        }

        [JsonConstructor]
        protected ChosenAnswerDeviceClassCondition()
        {
        }

        public bool IsFulfilled(DeviceClassificationQuestionnaireTree questionnaireTree)
        {
            //var isFromGivenQuestionnaire = questionnaireTree.Conditions.Any(x => x == this);
            //if (isFromGivenQuestionnaire)
            //{
            //    throw new InvalidOperationException();
            //}

            bool isSatisfied = true;

            foreach (KeyValuePair<string, string> entry in QuestionIdsWithExpectedChosenAnswerIds)
            {
                var question = questionnaireTree.FindQuestionOrThrow(entry.Key);
                var answer = question.Answers.FirstOrDefault(a => a.Id == entry.Value);

                if (answer == null)
                {
                    throw new InvalidOperationException($"Answer not found with ID: {entry.Value} from Question with ID: {entry.Key}");
                }

                if (answer.IsChosen != true)
                {
                    isSatisfied = false;
                }
            }

            return isSatisfied;
        }
    }
}