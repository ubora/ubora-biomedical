using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.Tasks.Events
{
    /// <summary>
    /// Marker interface for events of <see cref="ProjectTask"/>.
    /// </summary>
    public interface ITaskEvent : IAggregateMemberEvent, IProjectEvent
    {
    }
}