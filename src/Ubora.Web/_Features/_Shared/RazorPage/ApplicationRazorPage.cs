using Microsoft.AspNetCore.Mvc.Razor;

namespace Ubora.Web._Features._Shared.RazorPage
{
    public abstract class ApplicationRazorPage<T> : RazorPage<T>
    {
        public NoticeQueue Notices => new NoticeQueue(this.TempData);
    }

    public abstract class ApplicationRazorPage : ApplicationRazorPage<dynamic>
    {
    }
}
