using System;
using Newtonsoft.Json;

namespace Ubora.Domain.Questionnaires
{
    public abstract class AnswerBase
    {
        public string Id { get; protected set; }
        [JsonProperty("Next")]
        public string NextQuestionId { get; protected set; } // Can be null.
        public bool? IsChosen { get; private set; }

        internal void SetIsChosen(bool isChosen)
        {
            if (IsChosen.HasValue)
            {
                throw new InvalidOperationException();
            }
            IsChosen = isChosen;
        }
    }
}