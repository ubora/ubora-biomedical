using Ubora.Domain.Infrastructure.Events;
namespace Ubora.Domain.Projects.Tasks.Events
{
    /// <summary>
    /// Marker interface for events of <see cref="ProjectTask"/>.
    /// </summary>
    public interface ITaskEvent : IAggregateMemberEvent
    {
    }
}