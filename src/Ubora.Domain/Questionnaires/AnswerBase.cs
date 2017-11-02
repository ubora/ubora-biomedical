using System;

namespace Ubora.Domain.Questionnaires
{
    public abstract class AnswerBase
    {
        public string Id { get; protected set; }
        public string NextQuestionId { get; protected set; } // Can be null.
        public DateTime? AnsweredAt { get; private set; }
        public bool? IsChosen { get; private set; }

        internal void SetIsChosen(bool isChosen, DateTime at)
        {
            if (IsChosen.HasValue)
            {
                throw new InvalidOperationException();
            }
            IsChosen = isChosen;
            AnsweredAt = at;
        }
    }
}