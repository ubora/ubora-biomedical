using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Newtonsoft.Json;
using Ubora.Domain.Questionnaires.DeviceClassifications.DeviceClasses;

namespace Ubora.Domain.Questionnaires.DeviceClassifications
{
    public class DeviceClassificationQuestionnaireTree : QuestionnaireTreeBase<Question, Answer>
    {
        public DeviceClassificationQuestionnaireTree(IEnumerable<Question> questions, IEnumerable<DeviceClassWithConditions> deviceClassesWithConditions)
        {
            if (questions == null) throw new ArgumentNullException(nameof(questions));
            if (deviceClassesWithConditions == null) throw new ArgumentNullException(nameof(deviceClassesWithConditions));

            Questions = questions.ToImmutableArray();
            DeviceClassesWithConditions = deviceClassesWithConditions.ToImmutableArray();

            ValidateDeviceClassConditions(DeviceClassesWithConditions);
        }

        [JsonConstructor]
        protected DeviceClassificationQuestionnaireTree()
        {
        }

        public ImmutableArray<DeviceClassWithConditions> DeviceClassesWithConditions { get; private set; }

        public virtual DeviceClass GetHighestRiskDeviceClass()
        {
            var applicableDeviceClasses = DeviceClassesWithConditions
                .Where(x => x.IsAnyConditionSatisfied(this))
                .Select(x => x.DeviceClass)
                .ToList();

            if (!applicableDeviceClasses.Any())
            {
                return DeviceClass.None;
            }

            return applicableDeviceClasses.Max();
        }

        private void ValidateDeviceClassConditions(IEnumerable<DeviceClassWithConditions> deviceClassesWithConditions)
        {
            var deviceClassConditions = deviceClassesWithConditions.SelectMany(x => x.ChosenAnswerDeviceClassConditions);
            foreach (var condition in deviceClassConditions)
            {
                condition.Validate(this);
            }
        }
    }
}
