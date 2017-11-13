using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Ubora.Domain.Questionnaires.DeviceClassifications.DeviceClasses
{
    public class DeviceClass : IComparable<DeviceClass>
    {
        // Keeping this as a concrete class to lessen the JSON-serialization cost ('$type' field for each condition would be too expensive). 
        // If new types of conditions are introduced, add them as new properties.
        public ChosenAnswerDeviceClassCondition[] Conditions { get; private set; }

        public DeviceClass(IEnumerable<ChosenAnswerDeviceClassCondition> conditions)
        {
            Conditions = conditions.ToArray();
        }

        [JsonIgnore]
        public string Name { get; protected set; }

        [JsonIgnore]
        public int Weight { get; protected set; }

        internal bool HasAnyHits(DeviceClassificationQuestionnaireTree questionnaireTree)
        {
            return Conditions.Any(x => x.IsFulfilled(questionnaireTree));
        }

        internal int GetHits(DeviceClassificationQuestionnaireTree questionnaireTree)
        {
            return Conditions.Select(x => x.IsFulfilled(questionnaireTree)).Count();
        }

        public int CompareTo(DeviceClass other)
        {
            return Weight.CompareTo(other.Weight);
        }

        public void ValidateConditions(DeviceClassificationQuestionnaireTree questionnaireTree)
        {
            foreach (var condition in Conditions)
            {
                condition.Validate(questionnaireTree);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(DeviceClass))
            {
                return false;
            }
            return Equals((DeviceClass)obj);
        }

        protected bool Equals(DeviceClass other)
        {
            return string.Equals(Name, other.Name) && Weight == other.Weight;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() * 397 ^ Weight;
        }
    }
}