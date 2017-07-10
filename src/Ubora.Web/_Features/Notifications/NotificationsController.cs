using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Notifications;
using Ubora.Web.Infrastructure;

namespace Ubora.Web._Features.Notifications
{
    [Authorize]
    public class NotificationsController : UboraController
    {
        [RestoreModelStateFromTempData]
        public IActionResult Index([FromServices]IndexViewModel.Factory modelFactory)
        {
            var indexViewModel = modelFactory.Create(UserInfo.UserId);

            MarkNotificationsAsViewed();

            return View(indexViewModel);
        }

        public IActionResult History([FromServices]HistoryViewModel.Factory modelFactory)
        {
            var viewModel = modelFactory.Create(UserInfo.UserId);

            return View(viewModel);
        }

        private void MarkNotificationsAsViewed()
        {
            ExecuteUserCommand(new MarkNotificationsAsViewedCommand());
        }
    }
}