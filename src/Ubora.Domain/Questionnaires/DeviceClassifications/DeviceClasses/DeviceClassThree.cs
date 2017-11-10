using System.Collections.Generic;

namespace Ubora.Domain.Questionnaires.DeviceClassifications.DeviceClasses
{
    public class DeviceClassThree : DeviceClass
    {
        public DeviceClassThree(IEnumerable<ChosenAnswerDeviceClassCondition> conditions) : base(conditions)
        {
            Name = "III";
            Weight = 30;
        }
    }
}