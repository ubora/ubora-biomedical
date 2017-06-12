using System;
using System.Collections.Generic;
using System.Linq;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Notifications;
using Ubora.Domain.Projects;

namespace Ubora.Web._Features.Notifications
{
    public class HistoryViewModel
    {
        public List<HistoryInvitationViewModel> Invitations { get; set; } = new List<HistoryInvitationViewModel>();

        public class Factory
        {
            private readonly IQueryProcessor _processor;
            public Factory(IQueryProcessor processor)
            {
                _processor = processor;
            }

            public HistoryViewModel Create(Guid userId)
            {
                var historyViewModel = new HistoryViewModel();
                historyViewModel.Invitations = GetHistoryInvitationViewModels(userId);

                return historyViewModel;
            }

            private List<HistoryInvitationViewModel> GetHistoryInvitationViewModels(Guid userId)
            {
                var invitations = _processor.Find<InvitationToProject>()
                    .Where(x => x.InvitedMemberId == userId && x.IsAccepted != null);
                var invitationViewModels = new List<HistoryInvitationViewModel>();

                foreach (var invitation in invitations)
                {
                    var invitationViewModel = new HistoryInvitationViewModel();

                    var project = _processor.FindById<Project>(invitation.ProjectId);
                    invitationViewModel.ProjectTitle = project.Title;
                    invitationViewModel.WasAccepted = invitation.IsAccepted.Value;

                    invitationViewModels.Add(invitationViewModel);
                }

                return invitationViewModels;
            }
        }
    }

    public class HistoryInvitationViewModel
    {
        public string ProjectTitle { get; set; }
        public bool WasAccepted { get; set; }
    }
}
