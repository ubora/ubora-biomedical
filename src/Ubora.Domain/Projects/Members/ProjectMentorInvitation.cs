using System;
using Newtonsoft.Json;
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

        [JsonConstructor]
        private ProjectMentorInvitation(Guid id, Guid notificationTo, DateTime createdAt, Guid inviteeUserId, Guid projectId, Guid invitedBy) 
            : base(id, notificationTo, createdAt)
        {
            InviteeUserId = inviteeUserId;
            ProjectId = projectId;
            InvitedBy = invitedBy;
        }

        public Guid InviteeUserId { get; }
        public Guid InvitedBy { get; }
        public Guid ProjectId { get; }
    }
}