using System;

namespace Ubora.Domain.Projects
{
    public interface IProjectEntity
    {
        Guid Id { get; }
        Guid ProjectId { get; }
    }
}
