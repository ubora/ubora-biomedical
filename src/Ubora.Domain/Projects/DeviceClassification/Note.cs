using System;

namespace Ubora.Domain.Projects.DeviceClassification
{
    public class Note
    {
        public Guid Id { get; private set; }
        public string Value { get; private set; }

        public Note(string value)
        {
            Id = Guid.NewGuid();
            Value = value;
        }
    }
}
