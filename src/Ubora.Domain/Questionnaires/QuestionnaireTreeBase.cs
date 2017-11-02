using System;
using System.Linq;
using Newtonsoft.Json;

namespace Ubora.Domain.Questionnaires
{
    // Inherit this, implement constructor.
    public abstract class QuestionnaireTreeBase<TQuestion, TAnswer> 
        where TQuestion : QuestionBase<TAnswer> 
        where TAnswer : AnswerBase
    {
        [JsonProperty(nameof(Questions))]
        private TQuestion[] _questions;

        [JsonIgnore]
        public TQuestion[] Questions
        {
            get { return _questions; }
            protected set
            {
                if (_questions != null) throw new InvalidOperationException("Let's keep this immutable.");
                ValidateQuestions(value);
                _questions = value;
            }
        }

        public virtual TQuestion FindQuestionOrThrow(string id)
        {
            var question = Questions.SingleOrDefault(x => x.Id == id);
            if (question == null)
            {
                throw new InvalidOperationException($"Question not found with ID: {id}");
            }

            return question;
        }

        public virtual TQuestion FindNextUnansweredQuestion()
        {
            var question = Questions.First();

            while (question.IsAnswered)
            {
                var nextQuestionId = question.Answers.SingleOrDefault(a => a.IsChosen == true)?.NextQuestionId;
                if (nextQuestionId == null)
                {
                    return null;
                }

                question = FindQuestionOrThrow(nextQuestionId);
            }

            return question;
        }

        public virtual TQuestion FindPreviousAnsweredQuestionFrom(TQuestion question)
        {
            ThrowIfQuestionNotFromQuestionnaire(question);

            return FindClosestAnsweredQuestion(question, Direction.Previous);
        }

        public virtual TQuestion FindNextQuestionFromAnsweredQuestion(TQuestion question)
        {
            if (!question.IsAnswered)
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

        private void ThrowIfQuestionNotFromQuestionnaire(TQuestion question)
        {
            var isFromQuestionnaire = Questions.Any(q => q == question);
            if (!isFromQuestionnaire)
            {
                throw new InvalidOperationException("Given question not found from this questionnaire.");
            }
        }

        // I've decided to validate the whole tree of questions instead of validating questions individually inside constructors.
        protected virtual void ValidateQuestions(TQuestion[] questions)
        {
            if (questions == null) throw new ArgumentNullException(nameof(questions));

            // Validate all questions have answers
            var questionsWithoutAnswers = questions.Where(q => q.Answers == null || !q.Answers.Any()).Select(q => q.Id);
            if (questionsWithoutAnswers.Any())
            {
                throw new InvalidOperationException($"Answers not found for questions with ID-s: {string.Join(",", questionsWithoutAnswers)}");
            }

            // Validate no duplicate ID-s
            var duplicateQuestionIds = questions.GroupBy(q => q.Id).Where(g => g.Count() > 1).Select(g => g.Key);
            if (duplicateQuestionIds.Any())
            {
                throw new InvalidOperationException($"Following question ID-s are duplicated: {string.Join(",", duplicateQuestionIds)}");
            }

            // Validate all next questions are present from answers
            var answersWithNextQuestion = questions.SelectMany(q => q.Answers)
                .Where(a => a.NextQuestionId != null);

            foreach (var answer in answersWithNextQuestion)
            {
                var nextQuestion = questions.SingleOrDefault(q => q.Id == answer.NextQuestionId);
                if (nextQuestion == null)
                {
                    throw new InvalidOperationException($"Next question not found with ID: {answer.NextQuestionId}");
                }
            }

            // Validate no answer refers to its own parent question
            var questionIdsThatHaveInvalidAnswers = questions.Where(q => q.Answers.Any(a => a.NextQuestionId != null && a.NextQuestionId == q.Id)).Select(q => q.Id);
            if (questionIdsThatHaveInvalidAnswers.Any())
            {
                throw new InvalidOperationException($"Questions with these ID-s have answers that refer to parent question: {string.Join(",", questionIdsThatHaveInvalidAnswers)}");
            }
        }

        private TQuestion FindClosestAnsweredQuestion(TQuestion question, Direction direction)
        {
            var questions = Questions;

            if (direction.IsNext)
            {
                questions = questions.Reverse().ToArray();
            }

            TQuestion previous = null;
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