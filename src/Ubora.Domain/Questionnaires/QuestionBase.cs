using System;
using System.Linq;
using Newtonsoft.Json;

namespace Ubora.Domain.Questionnaires
{
    public abstract class QuestionBase<TAnswer> where TAnswer : AnswerBase
    {
        public string Id { get; protected set; }
        public TAnswer[] Answers { get; protected set; }

        [JsonIgnore]
        public bool IsAnswered => Answers.Any(x => x.IsChosen.HasValue);

        public void ChooseAnswer(string answerId)
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

            answer.IsChosen = true;

            var otherAnswers = Answers.Where(x => x.Id != answer.Id);
            foreach (var otherAnswer in otherAnswers)
            {
                otherAnswer.IsChosen = false;
            }
        }


        [JsonIgnore]
        public TAnswer ChosenAnswer => Answers.SingleOrDefault(a => a.IsChosen == true);
    }
}