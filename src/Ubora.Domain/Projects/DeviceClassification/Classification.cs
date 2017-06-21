using System;
using System.Collections.Generic;

namespace Ubora.Domain.Projects.DeviceClassification
{
    public class Classification
    {
        public Guid Id { get; private set; }
        public string Text { get; private set; }
        public List<Guid> QuestionIds { get; private set; }
        public int Weight { get; private set; }

        public Classification(string text, int weight, List<Guid> questionIds)
        {
            Id = Guid.NewGuid();
            Text = text;
            Weight = weight;
            QuestionIds = questionIds;
        }

        public static bool operator <(Classification classification1, Classification classification2) => classification1.Weight < classification2.Weight;
        public static bool operator >(Classification classification1, Classification classification2) => classification1.Weight > classification2.Weight;
    }
}