using System;
using System.Collections.Generic;
using System.Linq;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Notifications;
using Ubora.Domain.Projects;

namespace Ubora.Web._Features.Notifications
{
    public class IndexViewModel
    {
        public List<IndexInvitationViewModel> Invitations { get; set; } = new List<IndexInvitationViewModel>();

        public class Factory
        {
            private readonly IQueryProcessor _processor;
            public Factory(IQueryProcessor processor)
            {
                _processor = processor;
            }

            public IndexViewModel Create(Guid userId)
            {
                var indexViewModel = new IndexViewModel();
                indexViewModel.Invitations = GetIndexInvitationViewModels(userId);

                return indexViewModel;
            }

            private List<IndexInvitationViewModel> GetIndexInvitationViewModels(Guid userId)
            {
                var invitations = _processor.Find<InvitationToProject>()
                    .Where(x => x.InvitedMemberId == userId && !x.IsAccepted && !x.IsDeclined);
                var invitationViewModels = new List<IndexInvitationViewModel>();

                foreach (var invitation in invitations)
                {
                    var invitationViewModel = new IndexInvitationViewModel();

                    var project = _processor.FindById<Project>(invitation.ProjectId);
                    invitationViewModel.ProjectTitle = project.Title;
                    invitationViewModel.InviteId = invitation.Id;
                    invitationViewModel.IsUnread = invitation.IsNotViewed;

                    invitationViewModels.Add(invitationViewModel);
                }

                return invitationViewModels;
            }
        }
    }

    public class IndexInvitationViewModel
    {
        public string ProjectTitle { get; set; }
        public Guid InviteId { get; set; }
        public bool IsUnread { get; set; }
    }
}