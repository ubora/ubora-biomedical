using System;
using Ubora.Domain.Notifications;

namespace Ubora.Domain.Projects.Members
{
    public class ProjectMentorInvitation : UserBinaryAction, IProjectEntity
    {
        public ProjectMentorInvitation(Guid inviteeUserId, Guid projectId, Guid invitedBy) : base(inviteeUserId)
        {
            InviteeUserId = inviteeUserId;
            ProjectId = projectId;
            InvitedBy = invitedBy;
        }

        public Guid InviteeUserId { get; private set; }
        public Guid InvitedBy { get; private set; }
        public Guid ProjectId { get; private set; }
    }
}