using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.Assignments.Events
{
    /// <summary>
    /// Marker interface for events of <see cref="Assignment"/>.
    /// </summary>
    public interface IAssignmentEvent : IAggregateMemberEvent
    {
    }
}