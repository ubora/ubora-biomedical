using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Resources.Events
{
    public class ResourcePageBodyEditedEvent : UboraEvent
    {
        public ResourcePageBodyEditedEvent(UserInfo initiatedBy, QuillDelta body, int previousBodyVersion) 
            : base(initiatedBy)
        {
            Body = body;
            PreviousBodyVersion = previousBodyVersion;
        }

        public QuillDelta Body { get; }
        public int PreviousBodyVersion { get; }

        public override string GetDescription() => "Edited resource content.";
    }
}