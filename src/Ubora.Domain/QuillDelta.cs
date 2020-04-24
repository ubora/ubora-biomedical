using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Newtonsoft.Json;

namespace Ubora.Domain
{
    public class QuillDelta : ValueObject
    {
        private string _value;

        [JsonIgnore]
        public string Value => _value ?? (_value = CompressedValue != null ? CompressedValue.Decompress() : RawValue);

        [JsonProperty(PropertyName = "Value", NullValueHandling = NullValueHandling.Ignore)]
        protected string RawValue { get; set; }

        [JsonProperty]
        protected string CompressedValue { get; set; }

        [JsonConstructor]
        protected QuillDelta(string rawValue, string compressedValue)
        {
            RawValue = rawValue;
            CompressedValue = compressedValue;
        }

        public QuillDelta(string value = null)
        {
            value = value ?? "{\"ops\":[{\"insert\":\"\\n\"}]}";

            if (!(value.StartsWith('{') && value.EndsWith('}')))
            {
                throw new ArgumentException("Not JSON");
            }

            CompressedValue = value.Compress();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
