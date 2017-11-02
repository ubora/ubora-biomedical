using System;
using Newtonsoft.Json;

namespace Ubora.Domain.Questionnaires
{
    public abstract class AnswerBase
    {
        public string Id { get; protected set; }
        public string NextQuestionId { get; protected set; } // Can be null.

        [JsonProperty(nameof(IsChosen))]
        private bool? _isChosen;

        [JsonIgnore]
        public bool? IsChosen
        {
            get { return _isChosen; }
            internal set
            {
                if (IsChosen.HasValue)
                {
                    throw new InvalidOperationException();
                }
                _isChosen = value;
            }
        }
    }
}