using System;

namespace Ubora.Domain.Notifications.Join
{
    public class RequestToJoinProject : BaseNotification
    {
        public RequestToJoinProject(Guid id, Guid inviteTo, Guid askingToJoinMemberId, Guid projectId) : base(id, inviteTo)
        {
            AskingToJoinMemberId = askingToJoinMemberId;
            ProjectId = projectId;
        }

        public Guid AskingToJoinMemberId { get; }
        public Guid ProjectId { get; }
        public bool? IsAccepted
        {
            get
            {
                if (Accepted != null && Declined == null)
                {
                    return true;
                }
                if (Declined != null && Accepted == null)
                {
                    return false;
                }

                return null;
            }
        }
        public DateTime? Accepted { get; internal set; }
        public DateTime? Declined { get; internal set; }
        public override bool InHistory => Accepted != null || Declined != null;
    }
}
