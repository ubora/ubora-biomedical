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

        public Questionnaire(Question firstQuestion)
        {
            _firstQuestion = firstQuestion ?? throw new ArgumentNullException();
        }

        [JsonConstructor]
        protected Questionnaire()
        {
        }

        /// <returns>Returns NULL when all questions answered.</returns>
        public Question FindNextUnansweredQuestion()
        {
            return FindNextUnansweredFrom(_firstQuestion);
        }

        /// <summary>Finds the next unaswered question after the one given.</summary>
        private Question FindNextUnansweredFrom(Question question)
        {
            if (question == null) { throw new ArgumentNullException(nameof(question)); }

            var isUnanswered = (question.Answer == null);
            if (isUnanswered)
            {
                return question;
            }

            // Find next unanswered sub question ONLY when answered 'true'.
            if (question.Answer.Value == true)
            {
                var unansweredSubQuestion = FindNextUnansweredSubQuestion(question);
                if (unansweredSubQuestion != null)
                {
                    return unansweredSubQuestion;
                }
            }

            if (question.NextMainQuestion == null)
            {
                return null;
            }

            return FindNextUnansweredFrom(question.NextMainQuestion);
        }

        /// <remarks> Returns NULL if none found. </remarks>>
        private Question FindNextUnansweredSubQuestion(Question question)
        {
            if (!question.SubQuestions.Any())
            {
                return null;
            }

            var lastQuestionWithAnswer = question.SubQuestions.LastOrDefault(x => x.Answer.HasValue);

            // When last question was answered 'true' then look for it's sub questions (recursion).
            if (lastQuestionWithAnswer != null
                && lastQuestionWithAnswer.Answer == true)
            {
                var levelDeeperUnansweredQuestion = FindNextUnansweredSubQuestion(lastQuestionWithAnswer);
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
                yield return question;

                if (question.SubQuestions.Any())
                {
                    // Bit 'weird' foreach to enable yield-return.
                    foreach (var subQuestion in GetAllSubQuestions(question))
                    {
                        yield return subQuestion;
                    }
                }

                question = question.NextMainQuestion;
            }
        }

        public virtual IEnumerable<Question> GetAllAnsweredQuestions()
        {
            return GetAllQuestions().Where(x => x.Answer.HasValue);
        }

        private IEnumerable<Question> GetAllSubQuestions(Question parentQuestion)
        {
            for (int i = 0; i < parentQuestion.SubQuestions.Count(); i++)
            {
                var subQuestion = parentQuestion.SubQuestions.ElementAt(i);

                yield return subQuestion;

                // Return sub questions of the sub question (recursion).
                // Bit 'weird' foreach to enable yield-return.
                foreach (var subQuestionSubQuestion in GetAllSubQuestions(subQuestion))
                {
                    yield return subQuestionSubQuestion;
                }
            }
        }

        public virtual Question FindQuestionOrThrow(Guid id)
        {
            return GetAllQuestions().First(x => x.Id == id);
        }

        public virtual Question FindPreviousAnsweredQuestionFrom(Question question)
        {
            ThrowIfQuestionNotFromQuestionnaire(question);

            return FindClosestAnsweredQuestion(question, Direction.Previous);
        }

        public virtual Question FindNextQuestionFromAnsweredQuestion(Question question)
        {
            if (!question.Answer.HasValue)
            {
                throw new InvalidOperationException("Given question is not answered.");
            }

            ThrowIfQuestionNotFromQuestionnaire(question);

            var nextQuestion = FindClosestAnsweredQuestion(question, Direction.Next);
            if (nextQuestion != null)
            {
                return nextQuestion;
            }

            return FindNextUnansweredQuestion();
        }

        private void ThrowIfQuestionNotFromQuestionnaire(Question question)
        {
            var isFromQuestionnaire = GetAllQuestions().Any(q => q == question);
            if (!isFromQuestionnaire)
            {
                throw new InvalidOperationException("Given question not found from this questionnaire.");
            }
        }

        private Question FindClosestAnsweredQuestion(Question question, Direction direction)
        {
            var questions = GetAllAnsweredQuestions();

            if (direction.IsNext)
            {
                questions = questions.Reverse();
            }

            Question previous = null;
            foreach (var q in questions)
            {
                if (q == question)
                {
                    break;
                }
                previous = q;
            }
            return previous;
        }

        private class Direction
        {
            public bool IsNext { get; private set; }
            public bool IsPrevious => !IsNext;

            private Direction()
            {
            }

            public static Direction Next => new Direction { IsNext = true };
            public static Direction Previous => new Direction { IsNext = false };
        }
    }
}