namespace Ubora.Domain.Questionnaires.DeviceClassifications
{
    public class DeviceClass
    {
        public static DeviceClass One = new DeviceClass { Name = "I" };
        public static DeviceClass TwoA = new DeviceClass { Name = "IIa" };
        public static DeviceClass TwoB = new DeviceClass { Name = "IIb" };
        public static DeviceClass Three = new DeviceClass { Name = "III" };

        public string Name { get; private set; }

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
            return string.Equals(Name, other.Name);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}