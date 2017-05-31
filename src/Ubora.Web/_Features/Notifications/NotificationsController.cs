using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Notifications;
using Ubora.Domain.Projects;

namespace Ubora.Web._Features.Notifications
{
    [Authorize]
    public class NotificationsController : UboraController
    {
        public NotificationsController(ICommandQueryProcessor processor) : base(processor)
        {
        }

        public IActionResult Index()
        {
            var indexViewModel = new IndexViewModel();

            indexViewModel.Invitations = GetIndexInvitationViewModels();

            MarkInvitationsAsViewed();

            return View(indexViewModel);
        }

        public IActionResult History()
        {
            var indexViewModel = new HistoryViewModel();
            indexViewModel.Invitations = GetHistoryInvitationViewModels();

            return View(indexViewModel);
        }

        // TODO: Factory?
        private List<HistoryInvitationViewModel> GetHistoryInvitationViewModels()
        {
            var invitations = Find<InvitationToProject>()
                .Where(x => x.InvitedMemberId == UserInfo.UserId && (x.IsAccepted || x.IsDeclined));
            var invitationViewModels = new List<HistoryInvitationViewModel>();

            foreach (var invitation in invitations)
            {
                var invitationViewModel = new HistoryInvitationViewModel();

                var project = FindById<Project>(invitation.ProjectId);
                invitationViewModel.ProjectTitle = project.Title;
                invitationViewModel.WasAccepted = invitation.IsAccepted;

                invitationViewModels.Add(invitationViewModel);
            }

            return invitationViewModels;
        }
        
        // TODO: Factory?
        private List<IndexInvitationViewModel> GetIndexInvitationViewModels()
        {
            var invitations = Find<InvitationToProject>()
                .Where(x => x.InvitedMemberId == UserInfo.UserId && !x.IsAccepted && !x.IsDeclined);
            var invitationViewModels = new List<IndexInvitationViewModel>();

            foreach (var invitation in invitations)
            {
                var invitationViewModel = new IndexInvitationViewModel();

                var project = FindById<Project>(invitation.ProjectId);
                invitationViewModel.ProjectTitle = project.Title;
                invitationViewModel.InviteId = invitation.Id;
                invitationViewModel.IsUnread = invitation.IsNotViewed;

                invitationViewModels.Add(invitationViewModel);
            }

            return invitationViewModels;
        }

        private void MarkInvitationsAsViewed()
        {
            var invitations = Find<InvitationToProject>()
                    .Where(x => x.InvitedMemberId == UserInfo.UserId && x.IsNotViewed);

            foreach (var invitation in invitations)
            {
                ExecuteUserCommand(new UpdateMemberInvitationToProjectAsViewedCommand { InvitationId = invitation.Id });
            }
        }
    }
}