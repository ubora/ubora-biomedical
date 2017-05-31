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
        public InvitationToProjectState State { get; internal set; }

        public bool IsAccepted => State == InvitationToProjectState.Accepted;
        public bool IsDeclined => State == InvitationToProjectState.Declined;
        public bool IsNotViewed => State == InvitationToProjectState.None;
    }

    public enum InvitationToProjectState
    {
        None,
        Accepted,
        Declined,
        Viewed
    }
}
