using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Ubora.Web.Infrastructure
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class FileSizeAttribute : ValidationAttribute
    {
        public long MaxBytes { get; set; }
        public string FileTooLargeMessage { get; set; }

        public FileSizeAttribute(long maxBytes)
        {
            MaxBytes = maxBytes;
        }

        public FileSizeAttribute(long maxBytes, string fileTooLargeMessage)
        {
            MaxBytes = maxBytes;
            FileTooLargeMessage = fileTooLargeMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Please select a file to upload!");
            }

            var file = (IFormFile)value;

            if (file.Length > MaxBytes)
            {
                var message = string.IsNullOrEmpty(FileTooLargeMessage) ? "Please upload a smaller file!" : FileTooLargeMessage;
                return new ValidationResult(message);
            }

            return ValidationResult.Success;
        }
    }
}
