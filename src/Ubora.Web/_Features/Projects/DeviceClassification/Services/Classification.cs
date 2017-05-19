using System;
using System.Collections.Generic;

namespace Ubora.Web._Features.Projects.DeviceClassification.Services
{
    public class Classification
    {
        public Guid Id { get; set; }
        public string Text { get; }
        public List<Guid> QuestionIds { get; }

        public Classification(Guid id, string text, List<Guid> questionIds)
        {
            Id = id;
            Text = text;
            QuestionIds = questionIds;
        }
    }
}