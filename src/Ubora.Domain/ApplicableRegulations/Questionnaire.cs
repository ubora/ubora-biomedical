using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Ubora.Domain.ApplicableRegulations
{
    public class Questionnaire
    {
        [JsonProperty(nameof(_firstQuestion))]
        private readonly Question _firstQuestion;

        public Questionnaire(Guid id, Question firstQuestion)
        {
            Id = id;
            _firstQuestion = firstQuestion ?? throw new ArgumentNullException();
        }

        [JsonConstructor]
        protected Questionnaire()
        {
        }

        public Guid Id { get; private set; }

        /// <summary>
        /// A list of question follows. All main questions (number 1, 2, 3….) shall always be asked. 
        /// Subsequent questions (1.1; 1.1.1 and so on) shall be asked only if the answer to the main question was “YES”
        /// </summary>
        public Question GetNextUnanswered()
        {
            return GetNextUnansweredFrom(_firstQuestion);
        }

        private Question GetNextUnansweredFrom(Question question)
        {
            if (question == null)
            {
                return null;
            }

            if (question.Answer == null)
            {
                return question;
            }

            if (question.Answer.Value == true)
            {
                var unansweredQuestionBelow = FindNextUnansweredQuestionBelow(question);
                if (unansweredQuestionBelow != null)
                {
                    return unansweredQuestionBelow;
                }
            }

            return GetNextUnansweredFrom(question.NextQuestion);
        }

        /// <remarks> Returns NULL if none found. </remarks>>
        private Question FindNextUnansweredQuestionBelow(Question question)
        {
            if (!question.SubQuestions.Any())
            {
                return null;
            }

            var lastQuestionWithAnswer = question.SubQuestions.LastOrDefault(x => x.Answer.HasValue);

            if (lastQuestionWithAnswer != null 
                && lastQuestionWithAnswer.Answer == true)
            {
                var levelDeeperUnansweredQuestion = FindNextUnansweredQuestionBelow(lastQuestionWithAnswer);
                if (levelDeeperUnansweredQuestion != null)
                {
                    return levelDeeperUnansweredQuestion;
                }
            }

            return question.SubQuestions.FirstOrDefault(x => !x.Answer.HasValue);
        }

        public IEnumerable<Question> GetAllQuestions()
        {
            var question = _firstQuestion;

            while (question != null)
            {
                if (question.SubQuestions.Any())
                {
                    foreach (var subQuestion in GetAllQuestionsBelow(question))
                    {
                        yield return subQuestion;
                    }
                }

                yield return question;

                question = question.NextQuestion;
            }
        }

        private IEnumerable<Question> GetAllQuestionsBelow(Question parentQuestion)
        {
            for (int i = 0; i < parentQuestion.SubQuestions.Count(); i++)
            {
                var subQuestion = parentQuestion.SubQuestions.ElementAt(i);

                var questionsBelowSubQuestion = GetAllQuestionsBelow(subQuestion);
                foreach (var questionBelowSubQuestion in questionsBelowSubQuestion)
                {
                    yield return questionBelowSubQuestion;
                }

                yield return subQuestion;
            }
        }

        public Question FindQuestionOrThrow(Guid id)
        {
            return GetAllQuestions().First(x => x.Id == id);
        }
    }
}