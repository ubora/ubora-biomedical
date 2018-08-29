using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Ubora.Domain
{
    public class QuillDelta : ValueObject
    {
        public string Value { get; }

        public QuillDelta(string value = "{\"ops\":[{\"insert\":\"\\n\"}]}")
        {
            if (!(value.StartsWith('{') && value.EndsWith('}')))
            {
                throw new ArgumentException("Not JSON");
            }

            Value = value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}