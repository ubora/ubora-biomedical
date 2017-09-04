using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Ubora.Domain.ApplicableRegulations
{
    public class Question
    {
        public Question(string text, Question nextQuestion, IEnumerable<Question> subQuestions = null)
        {
            Id = Guid.NewGuid();
            Text = text;
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
        public string Text { get; private set; }

        [JsonProperty(nameof(Answer))]
        private bool? _answer;

        [JsonIgnore]
        public bool? Answer
        {
            get => _answer;
            set
            {
                if (Answer.HasValue)
                {
                    throw new InvalidOperationException("Already answered.");
                }
                _answer = value;
            }
        }
    }
}