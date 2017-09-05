using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Ubora.Domain.ApplicableRegulations.Texts;

namespace Ubora.Domain.ApplicableRegulations
{
    public class Question
    {
        public Question(string resourceName, Question nextQuestion, IEnumerable<Question> subQuestions = null)
        {
            Id = Guid.NewGuid();
            ResourceName = resourceName;
            NextQuestion = nextQuestion;
            SubQuestions = subQuestions ?? Enumerable.Empty<Question>();

            if (SubQuestions.Any(x => x.NextQuestion != this.NextQuestion))
            {
                throw new InvalidOperationException("Sub questions must have the same next question as the parent question.");
            }
        }

        [JsonConstructor]
        protected Question()
        {
        }

        public Guid Id { get; private set; }
        public Question NextQuestion { get; private set; }
        public IEnumerable<Question> SubQuestions { get; private set; }
        public string ResourceName { get; private set; }

        public bool? Answer { get; private set; }

        public string QuestionText => QuestionTexts.ResourceManager.GetString(this.ResourceName);
        public string AffirmativeAnswerText => IsoStandardTexts.ResourceManager.GetString(this.ResourceName);

        public void AnswerQuestion(bool answer)
        {
            if (this.Answer.HasValue)
            {
                throw new InvalidOperationException("Already answered.");
            }
            Answer = answer;
        }
    }
}