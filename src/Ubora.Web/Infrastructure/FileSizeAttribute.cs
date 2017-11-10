using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ubora.Web.Infrastructure
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class FileSizeAttribute : ValidationAttribute
    {
        public int MaxBytes { get; set; }
        public string FileTooLargeMessage { get; set; }

        public FileSizeAttribute(int maxBytes)
        {
            MaxBytes = maxBytes;
        }

        public FileSizeAttribute(int maxBytes, string fileTooLargeMessage)
        {
            MaxBytes = maxBytes;
            FileTooLargeMessage = fileTooLargeMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value as IFormFile != null)
            {
                return IsValidFileSize((IFormFile)value);
            }

            if (value as IEnumerable<IFormFile> != null)
            {
                return IsValidFileSizes((IEnumerable<IFormFile>)value);
            }

            return new ValidationResult("error");
        }

        private ValidationResult IsValidFileSize(IFormFile file)
        {
            if (file.Length > MaxBytes)
            {
                var message = string.IsNullOrEmpty(FileTooLargeMessage) ? "Please upload a smaller file!" : FileTooLargeMessage;
                return new ValidationResult(message);
            }

            return ValidationResult.Success;
        }

        private ValidationResult IsValidFileSizes(IEnumerable<IFormFile> files)
        {
            foreach (var file in files)
            {
                if (file.Length > MaxBytes)
                {
                    var message = string.IsNullOrEmpty(FileTooLargeMessage) ? "Please upload a smaller file!" : FileTooLargeMessage;
                    return new ValidationResult(message);
                }
            }

            return ValidationResult.Success;
        }
    }
}
