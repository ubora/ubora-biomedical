using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Ubora.Web.Services;

namespace Ubora.Web._Features.Notifications
{
    public class UnreadNotificationsViewComponent : ViewComponent
    {
        private readonly UnreadNotificationsViewModel.Factory _modelFactory;

        public UnreadNotificationsViewComponent(UnreadNotificationsViewModel.Factory modelFactory)
        {
            _modelFactory = modelFactory;
        }

#pragma warning disable 1998
        public async Task<IViewComponentResult> InvokeAsync()
#pragma warning restore 1998
        {
            var model = _modelFactory.Create(User.GetId());

            return View("~/_Features/Notifications/UnreadNotificationsPartial.cshtml", model);
        }
    }
}
