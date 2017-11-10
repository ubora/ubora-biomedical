using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Ubora.Domain.Questionnaires.ApplicableRegulations.Texts;

namespace Ubora.Domain.Questionnaires.ApplicableRegulations
{
    public class Question : QuestionBase<Answer>
    {
        public Question(string id, IEnumerable<Answer> answers)
        {
            Id = id;
            Answers = answers.ToArray();
        }

        [JsonConstructor]
        protected Question()
        {
        }

        [JsonIgnore]
        public virtual string QuestionText => QuestionTexts.ResourceManager.GetString(this.Id);
        /// <remarks>Bear in mind that this will go straight to the web client.</remarks>
        [JsonIgnore]
        public string IsoStandardHtmlText => IsoStandardTexts.ResourceManager.GetString(this.Id);
        [JsonIgnore]
        public string NoteText => NoteTexts.ResourceManager.GetString(this.Id);

        [JsonIgnore]
        public string ChosenAnswerText
        {
            get
            {
                var chosenAnswer = Answers.SingleOrDefault(x => x.IsChosen == true);
                if (chosenAnswer == null)
                {
                    return "";
                }
                return AnswerTexts.ResourceManager.GetString(chosenAnswer.Id);
            }
        }
    }
}