using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Notifications;

namespace Ubora.Web._Features.Notifications
{
    [Authorize]
    public class NotificationsController : UboraController
    {
        private readonly HistoryViewModel.Factory _historyViewModelFactory;
        private readonly IndexViewModel.Factory _indexViewModelFactory;

        public NotificationsController(
            ICommandQueryProcessor processor,
            HistoryViewModel.Factory historyViewModelFactory,
            IndexViewModel.Factory indexViewModelFactory) : base(processor)
        {
            _historyViewModelFactory = historyViewModelFactory;
            _indexViewModelFactory = indexViewModelFactory;
        }

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
            ExecuteUserCommand(new MarkNotificationsAsViewedCommand { UserId = UserInfo.UserId });
        }
    }
}