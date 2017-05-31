using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.Tasks
{
    /// <summary>
    /// Marker interface for events of <see cref="ProjectTask"/>.
    /// </summary>
    public interface ITaskEvent : IAggregateMemberEvent
    {
    }
}