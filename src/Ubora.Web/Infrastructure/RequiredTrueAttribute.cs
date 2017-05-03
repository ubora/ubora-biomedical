using System;
using System.ComponentModel.DataAnnotations;

namespace Ubora.Web.Infrastructure
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class RequiredTrueAttribute : ValidationAttribute
    {
        public RequiredTrueAttribute() : base("The field is required.")
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var valueBool = (bool)value;

            if (!valueBool)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
