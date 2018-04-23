using System;
using Newtonsoft.Json;
using Ubora.Domain.Notifications;

namespace Ubora.Domain.Projects.Members
{
    public class InvitationToProject : UserBinaryAction
    {
        public InvitationToProject(Guid inviteTo, Guid projectId) : base(inviteTo)
        {
            ProjectId = projectId;
            InvitedMemberId = inviteTo;
        }

        [JsonConstructor]
        private InvitationToProject(Guid id, Guid notificationTo, DateTime createdAt, Guid invitedMemberId, Guid projectId)
            : base(id, notificationTo, createdAt)
        {
            InvitedMemberId = invitedMemberId;
            ProjectId = projectId;
        }

        public Guid InvitedMemberId { get; }
        public Guid ProjectId { get; }
    }
}