using System;
using System.Collections.Generic;
using System.Linq;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Notifications;
using Ubora.Domain.Projects;
using Ubora.Domain.Users;

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

            protected Factory() { }

            public virtual HistoryViewModel Create(Guid userId)
            {
                var historyViewModel = new HistoryViewModel();
                historyViewModel.Invitations = GetHistoryInvitationViewModels(userId);

                return historyViewModel;
            }

            private List<HistoryInvitationViewModel> GetHistoryInvitationViewModels(Guid userId)
            {
                var invitations = _processor.Find(new UserInvitations(userId))
                    .Where(x => x.IsAccepted != null);
                var invitationViewModels = new List<HistoryInvitationViewModel>();

                foreach (var invitation in invitations)
                {
                    var project = _processor.FindById<Project>(invitation.ProjectId);
                    var fullName = _processor.FindById<UserProfile>(invitation.InvitedMemberId).FullName;

                    var invitationViewModel = new HistoryInvitationViewModel
                    {
                        ProjectTitle = project.Title,
                        ProjectId = project.Id,
                        WasAccepted = invitation.IsAccepted.Value,
                        UserFullName = fullName,
                        InviteId = invitation.Id,
                        IsCurrentUser = userId == invitation.InvitedMemberId
                    };

                    invitationViewModels.Add(invitationViewModel);
                }

                return invitationViewModels;
            }
        }
    }

    public class HistoryInvitationViewModel : BaseInvitationViewModel
    {
        public bool WasAccepted { get; set; }
    }
}
