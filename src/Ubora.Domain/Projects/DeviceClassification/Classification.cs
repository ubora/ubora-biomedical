using System;
using System.Collections.Generic;

namespace Ubora.Domain.Projects.DeviceClassification
{
    public class Classification
    {
        public Guid Id { get; }
        public string Text { get; }
        public List<Guid> QuestionIds { get; }
        private int Weight { get; }

        public Classification(Guid id, string text, int weight, List<Guid> questionIds)
        {
            Id = id;
            Text = text;
            Weight = weight;
            QuestionIds = questionIds;
        }

        public static bool operator <(Classification classification1, Classification classification2) => classification1.Weight < classification2.Weight;
        public static bool operator >(Classification classification1, Classification classification2) => classification1.Weight > classification2.Weight;
    }
}