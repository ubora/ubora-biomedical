using System;
using Newtonsoft.Json;
using Ubora.Domain.Notifications;

namespace Ubora.Domain.Projects.Members
{
    public class RequestToJoinProject : UserBinaryAction
    {
        public RequestToJoinProject(Guid inviteTo, Guid askingToJoinMemberId, Guid projectId) : base(inviteTo)
        {
            AskingToJoinMemberId = askingToJoinMemberId;
            ProjectId = projectId;
        }

        [JsonConstructor]
        private RequestToJoinProject(Guid id, Guid notificationTo, DateTime createdAt, Guid askingToJoinMemberId, Guid projectId)
            : base(id, notificationTo, createdAt)
        {
            AskingToJoinMemberId = askingToJoinMemberId;
            ProjectId = projectId;
        }

        public Guid AskingToJoinMemberId { get; }
        public Guid ProjectId { get; }
    }
}