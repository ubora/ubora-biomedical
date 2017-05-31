using System;
using System.ComponentModel.DataAnnotations;

namespace Ubora.Web.Infrastructure
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class NotDefaultAttribute : ValidationAttribute
    {
        public NotDefaultAttribute() : base((string)"The field can not have default value.")
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var type = value.GetType();
            var valueAsType = Convert.ChangeType(value, type);
            var @default = Activator.CreateInstance(type);

            if (valueAsType.Equals(@default))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
