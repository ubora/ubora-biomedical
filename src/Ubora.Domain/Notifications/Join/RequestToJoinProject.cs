using System;

namespace Ubora.Domain.Notifications.Join
{
    public class RequestToJoinProject : UserBinaryAction
    {
        public RequestToJoinProject(Guid inviteTo, Guid askingToJoinMemberId, Guid projectId) : base(inviteTo)
        {
            AskingToJoinMemberId = askingToJoinMemberId;
            ProjectId = projectId;
        }

        public Guid AskingToJoinMemberId { get; private set; }
        public Guid ProjectId { get; private set; }
    }
}