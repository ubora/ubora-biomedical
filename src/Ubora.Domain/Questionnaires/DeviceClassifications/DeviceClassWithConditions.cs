using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Ubora.Domain.Questionnaires.DeviceClassifications.DeviceClasses
{
    public class DeviceClassWithConditions
    {
        public DeviceClass DeviceClass { get; set; }

        // Keeping this as a concrete class to lessen the JSON-serialization cost ('$type' field for each condition would be too expensive). 
        // If new types of conditions are introduced, add them as new properties.
        public ChosenAnswerDeviceClassCondition[] ChosenAnswerDeviceClassConditions { get; set; }

        [JsonIgnore]
        public IEnumerable<IDeviceClassCondition> Conditions => ChosenAnswerDeviceClassConditions;

        public DeviceClassWithConditions(DeviceClass deviceClass, IEnumerable<IDeviceClassCondition> deviceClassConditions)
        {
            if (deviceClassConditions == null) throw new ArgumentNullException(nameof(deviceClassConditions));

            this.DeviceClass = deviceClass ?? throw new ArgumentNullException(nameof(deviceClass));

            // Warning: weird spot because of JSON size optimization
            this.ChosenAnswerDeviceClassConditions = deviceClassConditions.OfType<ChosenAnswerDeviceClassCondition>().ToArray();
        }

        [JsonConstructor]
        private DeviceClassWithConditions()
        {
        }

        internal bool IsAnyConditionSatisfied(DeviceClassificationQuestionnaireTree questionnaireTree)
        {
            return Conditions.Any(x => x.IsSatisfied(questionnaireTree));
        }

        internal int HowManyTimesConditionSatisfied(DeviceClassificationQuestionnaireTree questionnaireTree)
        {
            return Conditions.Select(x => x.IsSatisfied(questionnaireTree)).Count();
        }

        internal void ValidateConditions(DeviceClassificationQuestionnaireTree questionnaireTree)
        {
            foreach (var condition in Conditions)
            {
                condition.Validate(questionnaireTree);
            }
        }
    }
}