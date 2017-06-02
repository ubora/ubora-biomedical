using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Ubora.Web._Features._Shared
{
    public interface IModelStateUpdater
    {
        void UpdateFromValidationResult(ValidationResult src, ModelStateDictionary dest);
    }

    public class ModelStateUpdater : IModelStateUpdater
    {
        public void UpdateFromValidationResult(ValidationResult src, ModelStateDictionary dest)
        {
            foreach (var error in src.Errors)
            {
                if (dest.ContainsKey(error.Key))
                {
                    dest.Remove(error.Key);
                }
                foreach (var errorMessage in error.Value)
                {
                    dest.AddModelError(error.Key, errorMessage);
                }
            }
        }
    }
}
