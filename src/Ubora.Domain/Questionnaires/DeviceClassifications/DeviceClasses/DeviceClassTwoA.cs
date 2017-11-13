using System.Collections.Generic;

namespace Ubora.Domain.Questionnaires.DeviceClassifications.DeviceClasses
{
    public class DeviceClassTwoA : DeviceClass
    {
        public DeviceClassTwoA(IEnumerable<ChosenAnswerDeviceClassCondition> conditions) : base(conditions)
        {
            Name = "IIa";
            Weight = 20;
        }
    }
}