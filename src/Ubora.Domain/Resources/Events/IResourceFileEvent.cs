using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Resources.Events
{
    public interface IResourceFileEvent : IAggregateMemberEvent
    {
        Guid ResourcePageId { get; }
    }
}
