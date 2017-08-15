using System;
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

        public Guid AskingToJoinMemberId { get; private set; }
        public Guid ProjectId { get; private set; }
    }
}