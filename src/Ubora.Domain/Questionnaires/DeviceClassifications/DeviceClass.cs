using System;

namespace Ubora.Domain.Questionnaires.DeviceClassifications
{
    public class DeviceClass : IComparable<DeviceClass>
    {
        public static DeviceClass One = new DeviceClass { Name = "I", Weight = 100 };
        public static DeviceClass TwoA = new DeviceClass { Name = "IIa", Weight = 200 };
        public static DeviceClass TwoB = new DeviceClass { Name = "IIb", Weight = 250 };
        public static DeviceClass Three = new DeviceClass { Name = "III", Weight = 300 };

        public string Name { get; private set; }
        public int Weight { get; private set; }

        public int CompareTo(DeviceClass other)
        {
            return Weight.CompareTo(other.Weight);
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