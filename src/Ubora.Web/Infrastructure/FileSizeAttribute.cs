using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Ubora.Web.Infrastructure
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class FileSizeAttribute : ValidationAttribute
    {
        public int MaxBytes { get; set; }

        public FileSizeAttribute(int maxBytes)
            : base("Please upload a supported file.")
        {
            MaxBytes = maxBytes;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var file = (IFormFile)value;

            if (file == null)
            {
                return new ValidationResult("Please select a file to upload!");
            }

            if (file.Length > MaxBytes)
            {
                return new ValidationResult("Please upload a file of less than 4 MB!");
            }

            return ValidationResult.Success;
        }
    }
}
