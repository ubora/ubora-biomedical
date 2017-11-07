using System;
using System.ComponentModel.DataAnnotations;

namespace Ubora.Web._Features.Projects.DeviceClassifications
{
    public class AnswerDeviceClassificationQuestionPostModel
    {
        [Required]
        public string QuestionId { get; set; }
        [Required]
        public string AnswerId { get; set; }
        [Required]
        public Guid QuestionnaireId { get; set; }
    }
}