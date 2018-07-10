using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Resources.Events
{
    public class ResourcePageTitleChangedEvent : UboraEvent
    {
        public ResourcePageTitleChangedEvent(UserInfo initiatedBy, string title) : base(initiatedBy)
        {
            Title = title;
        }

        public string Title { get; }

        public override string GetDescription() => "changed resource page title.";
    }
}
