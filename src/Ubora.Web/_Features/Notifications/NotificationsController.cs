using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Notifications;
using Ubora.Web.Infrastructure;

namespace Ubora.Web._Features.Notifications
{
    [Authorize]
    public class NotificationsController : UboraController
    {
        private readonly HistoryViewModel.Factory _historyViewModelFactory;
        private readonly IndexViewModel.Factory _indexViewModelFactory;

        public NotificationsController(
            HistoryViewModel.Factory historyViewModelFactory,
            IndexViewModel.Factory indexViewModelFactory)
        {
            _historyViewModelFactory = historyViewModelFactory;
            _indexViewModelFactory = indexViewModelFactory;
        }

        [RestoreModelStateFromTempData]
        public IActionResult Index()
        {
            var indexViewModel = _indexViewModelFactory.Create(UserInfo.UserId);

            MarkNotificationsAsViewed();

            return View(indexViewModel);
        }

        public IActionResult History()
        {
            var viewModel = _historyViewModelFactory.Create(UserInfo.UserId);

            return View(viewModel);
        }

        private void MarkNotificationsAsViewed()
        {
            ExecuteUserCommand(new MarkNotificationsAsViewedCommand());
        }
    }
}