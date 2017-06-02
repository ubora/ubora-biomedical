using System.Collections.Generic;
using System.Linq;

namespace Ubora.Web._Features._Shared
{
    public class ValidationResult
    {
        public static ValidationResult WithError(string key, string message)
        {
            return new ValidationResult().AddError(key, message);
        }

        public static ValidationResult WithoutErrors => new ValidationResult();

        private Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

        public bool IsFailure => Errors.Any();

        public bool Success => !IsFailure;

        public Dictionary<string, List<string>> Errors
        {
            get { return _errors; }
            private set { _errors = value; }
        }

        public ValidationResult AddError(string key, string message)
        {
            if (!Errors.ContainsKey(key))
            {
                Errors[key] = new List<string>();
            }
            Errors[key].Add(message);
            return this;
        }

        public IEnumerable<string> OnlyMessages => Errors.SelectMany(e => e.Value);
    }
}
