using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Ubora.Domain.Resources
{
    public class QuillDelta : ValueObject
    {
        public string Value { get; }

        public QuillDelta(string value)
        {
            Value = value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}