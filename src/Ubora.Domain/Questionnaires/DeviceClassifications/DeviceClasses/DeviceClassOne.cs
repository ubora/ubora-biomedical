using System.Collections.Generic;

namespace Ubora.Domain.Questionnaires.DeviceClassifications.DeviceClasses
{
    public class DeviceClassOne : DeviceClass
    {
        public DeviceClassOne(IEnumerable<ChosenAnswerDeviceClassCondition> conditions) : base(conditions)
        {
            Name = "I";
            Weight = 10;
        }
    }
}