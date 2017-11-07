using System.Linq;
using Ubora.Domain.Questionnaires.DeviceClassifications;

namespace Ubora.Web._Features.Projects.DeviceClassifications
{
    public class DeviceClassificationReviewViewModel
    {
        public string ChosenClass { get; set; }
        public QuestionReviewViewModel[] Questions { get; set; }

        public class Factory
        {
            public DeviceClassificationReviewViewModel Create(DeviceClassificationAggregate aggregate)
            {
                var deviceClass = aggregate.QuestionnaireTree.GetDeviceClassHits().Max();
                return new DeviceClassificationReviewViewModel
                {
                    ChosenClass = deviceClass.Name,
                    Questions = aggregate.QuestionnaireTree.AnsweredQuestions.Select(q => new QuestionReviewViewModel
                    {
                        QuestionText = q.Text,
                        AnswerText = q.ChosenAnswer.Text
                    }).ToArray()
                };
            }
        }
    }

    public class QuestionReviewViewModel
    {
        public string QuestionText { get; set; }
        public string AnswerText { get; set; }
    }
}