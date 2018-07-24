using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.Linq;

namespace Ubora.Web.Infrastructure
{
    public static class MvcOptionsExtensions
    {
        public static void AddStringTrimmingProvider(this MvcOptions option)
        {
            var binderToFind = option.ModelBinderProviders
              .FirstOrDefault(x => x.GetType() == typeof(SimpleTypeModelBinderProvider));
            if (binderToFind == null)
            {
                return;
            }
            var index = option.ModelBinderProviders.IndexOf(binderToFind);
            option.ModelBinderProviders.Insert(index, new TrimmingModelBinderProvider());
        }
    }
}
