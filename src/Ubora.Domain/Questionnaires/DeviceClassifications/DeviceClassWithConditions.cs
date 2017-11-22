using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Ubora.Domain.Questionnaires.DeviceClassifications.DeviceClasses;

namespace Ubora.Domain.Questionnaires.DeviceClassifications
{
    public class DeviceClassWithConditions
    {
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

        public DeviceClass DeviceClass { get; private set; }

        // Keeping this as a concrete class to lessen the JSON-serialization cost ('$type' field for each condition would be too expensive). 
        // If new types of conditions are introduced, add them as new properties.
        [JsonProperty(nameof(ChosenAnswerDeviceClassConditions))]
        private ChosenAnswerDeviceClassCondition[] ChosenAnswerDeviceClassConditions { get; set; }

        [JsonIgnore]
        public IEnumerable<IDeviceClassCondition> Conditions => ChosenAnswerDeviceClassConditions;

        public bool IsAnyConditionSatisfied(DeviceClassificationQuestionnaireTree questionnaireTree)
        {
            return Conditions.Any(x => x.IsSatisfied(questionnaireTree));
        }

        public int HowManyTimesConditionSatisfied(DeviceClassificationQuestionnaireTree questionnaireTree)
        {
            return Conditions.Select(x => x.IsSatisfied(questionnaireTree)).Count();
        }

        public void ValidateConditions(DeviceClassificationQuestionnaireTree questionnaireTree)
        {
            foreach (var condition in Conditions)
            {
                condition.Validate(questionnaireTree);
            }
        }
    }
}