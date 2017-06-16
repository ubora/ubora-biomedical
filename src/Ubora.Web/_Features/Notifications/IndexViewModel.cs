using System;
using System.Collections.Generic;
using System.Linq;
using Ubora.Domain.Infrastructure.Queries;
using Ubora.Domain.Notifications;
using Ubora.Domain.Projects;
using Ubora.Domain.Users;

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

            protected Factory()
            {

            }

            public virtual IndexViewModel Create(Guid userId)
            {
                var indexViewModel = new IndexViewModel();
                indexViewModel.Invitations = GetIndexInvitationViewModels(userId);

                return indexViewModel;
            }

            private List<IndexInvitationViewModel> GetIndexInvitationViewModels(Guid userId)
            {
                var invitations = _processor.Find(new UserInvitations(userId))
                    .Where(x => x.IsAccepted == null);
                var invitationViewModels = new List<IndexInvitationViewModel>();

                foreach (var invitation in invitations)
                {
                    var project = _processor.FindById<Project>(invitation.ProjectId);
                    var fullName = _processor.FindById<UserProfile>(invitation.InvitedMemberId).FullName;

                    var viewModel = new IndexInvitationViewModel
                    {
                        ProjectTitle = project.Title,
                        InviteId = invitation.Id,
                        IsUnread = !invitation.HasBeenViewed,
                        ProjectId = project.Id,
                        UserFullName = fullName,
                        IsCurrentUser = userId == invitation.InvitedMemberId
                    };

                    invitationViewModels.Add(viewModel);
                }

                return invitationViewModels;
            }
        }
    }

    public class IndexInvitationViewModel : BaseInvitationViewModel
    {
        public bool IsUnread { get; set; }
    }
}