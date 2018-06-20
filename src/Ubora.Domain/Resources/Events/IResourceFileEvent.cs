using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Resources
{
    public interface IResourceFileEvent : IAggregateMemberEvent
    {
        Guid ResourcePageId { get; }
    }
}
