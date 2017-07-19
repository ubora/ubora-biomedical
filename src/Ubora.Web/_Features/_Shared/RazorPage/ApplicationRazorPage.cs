using Microsoft.AspNetCore.Mvc.Razor;

namespace Ubora.Web._Features._Shared.RazorPage
{
    public abstract class ApplicationRazorPage<T> : RazorPage<T>
    {
        public ApplicationRazorPageHelper<T> Web => new ApplicationRazorPageHelper<T>(this);
    }

    public abstract class ApplicationRazorPage : ApplicationRazorPage<dynamic>
    {
    }
}
