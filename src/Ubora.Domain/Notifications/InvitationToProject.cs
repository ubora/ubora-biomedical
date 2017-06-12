using Marten.Schema;
using System;

namespace Ubora.Domain.Notifications
{
    public class InvitationToProject
    {
        public InvitationToProject(Guid id)
        {
            Id = id;
        }

        [Identity]
        public Guid Id { get; }
        public Guid InvitedMemberId { get; internal set; }
        public Guid ProjectId { get; internal set; }
        public bool HasBeenViewed { get; internal set; }
        public bool? IsAccepted { get; set; }
    }
}
