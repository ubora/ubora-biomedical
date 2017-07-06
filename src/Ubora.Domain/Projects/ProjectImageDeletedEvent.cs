using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects
{
    internal class ProjectImageDeletedEvent : UboraEvent
    {
        public ProjectImageDeletedEvent(UserInfo initiatedBy) : base(initiatedBy)
        {
        }

        public override string GetDescription() => "Deleted project image";
    }
}
