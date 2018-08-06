using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Ubora.Domain
{
    public class QuillDelta : ValueObject
    {
        public string Value { get; }

        public QuillDelta(string value = "{\"ops\":[{\"insert\":\"\\n\"}]}")
        {
            Value = value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}