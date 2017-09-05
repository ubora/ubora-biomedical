using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Ubora.Domain.ApplicableRegulations.Texts;

namespace Ubora.Domain.ApplicableRegulations
{
    public class Question
    {
        /// <param name="resourceName">Used for getting appropriate texts for <see cref="QuestionText"/> and <see cref="IsoStandardHtmlText"/>.</param>
        /// <param name="nextMainQuestion"></param>
        /// <param name="subQuestions"></param>
        public Question(string resourceName, Question nextMainQuestion, IEnumerable<Question> subQuestions = null)
        {
            Id = Guid.NewGuid();
            ResourceName = resourceName;
            NextMainQuestion = nextMainQuestion;
            SubQuestions = subQuestions ?? Enumerable.Empty<Question>();

            if (SubQuestions.Any(x => x.NextMainQuestion != this.NextMainQuestion))
            {
                throw new InvalidOperationException("Sub questions must have the same next question as the parent question.");
            }
        }

        [JsonConstructor]
        protected Question()
        {
        }

        public Guid Id { get; private set; }
        public bool? Answer { get; private set; }
        public Question NextMainQuestion { get; private set; }
        public IEnumerable<Question> SubQuestions { get; private set; }

        public string ResourceName { get; private set; }
        public string QuestionText => QuestionTexts.ResourceManager.GetString(this.ResourceName);
        /// <remarks>Bear in mind that this will go straight to the web client.</remarks>
        public string IsoStandardHtmlText => IsoStandardTexts.ResourceManager.GetString(this.ResourceName);

        public void AnswerQuestion(bool answer)
        {
            if (Answer.HasValue)
            {
                throw new InvalidOperationException("Already answered.");
            }
            Answer = answer;
        }
    }
}