using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Ubora.Domain.Questionnaires.DeviceClassifications.DeviceClasses;

namespace Ubora.Domain.Questionnaires.DeviceClassifications
{
    public class DeviceClassificationQuestionnaireTree : QuestionnaireTreeBase<Question, Answer>
    {
        public DeviceClassificationQuestionnaireTree(Question[] questions, DeviceClass[] deviceClasses)
        {
            if (deviceClasses == null) throw new ArgumentNullException(nameof(deviceClasses));
            Questions = questions ?? throw new ArgumentNullException(nameof(questions));

            ValidateDeviceClasses(deviceClasses);
            DeviceClasses = deviceClasses;
        }

        [JsonConstructor]
        protected DeviceClassificationQuestionnaireTree()
        {
        }

        public DeviceClass[] DeviceClasses { get; private set; }

        public DeviceClass GetHeaviestDeviceClass()
        {
            var deviceClassesWithAnyHits = DeviceClasses.Where(dc => dc.HasAnyHits(this));
            var heaviestDeviceClass = deviceClassesWithAnyHits.Max();
            return heaviestDeviceClass;
        }

        private void ValidateDeviceClasses(IEnumerable<DeviceClass> deviceClasses)
        {
            foreach (var deviceClass in deviceClasses)
            {
                deviceClass.ValidateConditions(this);
            }
        }
    }
}
