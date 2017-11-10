using System.Collections.Generic;

namespace Ubora.Domain.Questionnaires.DeviceClassifications.DeviceClasses
{
    public class DeviceClassTwoB : DeviceClass
    {
        public DeviceClassTwoB(IEnumerable<ChosenAnswerDeviceClassCondition> conditions) : base(conditions)
        {
            Name = "IIb";
            Weight = 25;
        }
    }
}