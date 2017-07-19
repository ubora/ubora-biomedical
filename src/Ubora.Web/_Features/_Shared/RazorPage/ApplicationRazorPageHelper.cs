using System.Collections.Generic;
using System.Collections.ObjectModel;
using Ubora.Web._Features._Shared.Notices;

namespace Ubora.Web._Features._Shared.RazorPage
{
    public class ApplicationRazorPageHelper<T>
    {
        private readonly ApplicationRazorPage<T> _razorPage;

        public ApplicationRazorPageHelper(ApplicationRazorPage<T> razorPage)
        {
            _razorPage = razorPage;
        }

        private TempDataWrapper TempDataWrapper => new TempDataWrapper(_razorPage.TempData);

        public IReadOnlyList<Notice> Notices => new ReadOnlyCollection<Notice>(TempDataWrapper.Notices);
    }
}
