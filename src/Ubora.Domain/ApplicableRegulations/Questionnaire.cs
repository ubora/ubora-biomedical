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

        public Question FindQuestionOrThrow(Guid id)
        {
            return GetAllQuestions().First(x => x.Id == id);
        }

        // TODO throwaway logic
        //public Question FindQuestionBefore(Guid id)
        //{
        //    Question prev = null;
        //    foreach (var q in GetAllQuestions())
        //    {
        //        if (q.Id == id)
        //        {
        //            break;
        //        }
        //        prev = q;
        //    }

        //    if (prev == null)
        //    {

        //    }

        //    return prev;
        //}
    }
}