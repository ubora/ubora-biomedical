using System;
using System.Collections.Immutable;
using System.Linq;
using Newtonsoft.Json;

namespace Ubora.Domain.Questionnaires
{
    public abstract class QuestionBase<TAnswer> where TAnswer : AnswerBase
    {
        public string Id { get; protected set; }
        public ImmutableArray<TAnswer> Answers { get; protected set; }
        public DateTime? AnsweredAt { get; protected set; }

        [JsonIgnore]
        public bool IsAnswered => AnsweredAt.HasValue;

        [JsonIgnore]
        public TAnswer ChosenAnswer => Answers.SingleOrDefault(a => a.IsChosen == true);

        public void ChooseAnswer(string answerId, DateTime chosenAt)
        {
            if (IsAnswered)
            {
                throw new InvalidOperationException("Question already answered.");
            }

            var answer = Answers.SingleOrDefault(x => x.Id == answerId);
            if (answer == null)
            {
                throw new InvalidOperationException($"Answer not found with ID: {answerId}.");
            }

            answer.SetIsChosen(true);
            AnsweredAt = chosenAt;

            var otherAnswers = Answers.Where(x => x.Id != answer.Id);
            foreach (var otherAnswer in otherAnswers)
            {
                otherAnswer.SetIsChosen(false);
            }
        }
    }
}