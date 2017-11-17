using System;
using Newtonsoft.Json;

namespace Ubora.Domain.Questionnaires.DeviceClassifications.DeviceClasses
{
    public abstract class DeviceClass : IComparable<DeviceClass>
    {
        [JsonIgnore]
        public abstract string Name { get; }

        /// <summary>
        /// Used to compare device classes and to find the 'riskiest' or 'highest'.
        /// </summary>
        [JsonIgnore]
        public abstract int Weight { get; }

        public int CompareTo(DeviceClass other)
        {
            return Weight.CompareTo(other.Weight);
        }

        public static DeviceClass None => new DeviceClassNone();
        public static DeviceClass One => new DeviceClassOne();
        public static DeviceClass TwoA => new DeviceClassTwoA();
        public static DeviceClass TwoB => new DeviceClassTwoB();
        public static DeviceClass Three => new DeviceClassThree();

        internal DeviceClassWithConditions WithConditions(params IDeviceClassCondition[] deviceClassConditions)
        {
            return new DeviceClassWithConditions(this, deviceClassConditions);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
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