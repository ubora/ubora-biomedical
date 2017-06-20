using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.Repository
{
    /// <summary>
    /// Marker interface for events of <see cref="ProjectFile"/>.
    /// </summary>
    public interface IFileEvent : IAggregateMemberEvent
    {
    }
}
