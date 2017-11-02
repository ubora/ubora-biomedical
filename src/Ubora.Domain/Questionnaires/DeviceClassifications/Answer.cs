using Newtonsoft.Json;
using Ubora.Domain.Questionnaires.DeviceClassifications.Texts;

namespace Ubora.Domain.Questionnaires.DeviceClassifications
{
    public class Answer : AnswerBase
    {
        [JsonConstructor]
        protected Answer()
        {
        }

        public Answer(string id)
        {
            Id = id;
        }

        public Answer(string id, DeviceClass deviceClass)
        {
            Id = id;
            DeviceClass = deviceClass;
        }

        public Answer(string id, DeviceClass deviceClass, string nextQuestionId)
        {
            Id = id;
            NextQuestionId = nextQuestionId;
            DeviceClass = deviceClass;
        }

        public Answer(string id, string nextQuestionId)
        {
            Id = id;
            NextQuestionId = nextQuestionId;
        }

        private DeviceClass DeviceClass;

        /// <summary>
        /// Only relevant when constructing the questionnaire tree.
        /// </summary>
        public DeviceClass GetDeviceClass()
        {
            return DeviceClass;
        }

        public string Text => AnswerTexts.ResourceManager.GetString(this.Id);
    }
}