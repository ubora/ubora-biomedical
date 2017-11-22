﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Ubora.Domain.Questionnaires.DeviceClassifications
{
    public class ChosenAnswerDeviceClassCondition : IDeviceClassCondition
    {
        public ChosenAnswerDeviceClassCondition(Dictionary<string, string> questionAnswerIdPairs)
        {
            QuestionAnswerIdPairs = questionAnswerIdPairs ?? throw new ArgumentNullException(nameof(questionAnswerIdPairs));
        }

        /// <summary>
        /// Constructor for single question-answer condition.
        /// </summary>
        public ChosenAnswerDeviceClassCondition(string questionId, string answerId)
            : this(new Dictionary<string, string> { { questionId, answerId} })
        {
        }

        [JsonConstructor]
        protected ChosenAnswerDeviceClassCondition()
        {
        }

        /// <summary>
        /// Question-Answer ID pairs. If these questions are answered with the given answers then the condition will be fulfilled.
        /// Example: { "q1", "y" }, { "q2", "n" } => Condition fulfilled when question "q1" is answered with "y" and question "q2" answered with "n".
        /// </summary>
        [JsonProperty("qaIds")]
        private Dictionary<string, string> QuestionAnswerIdPairs { get; set; }

        public bool IsSatisfied(DeviceClassificationQuestionnaireTree questionnaireTree)
        {
            foreach (var entry in QuestionAnswerIdPairs)
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
            foreach (var entry in QuestionAnswerIdPairs)
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