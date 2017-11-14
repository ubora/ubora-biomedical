using System;
using System.Collections.Generic;
using System.Text;

namespace Ubora.Domain.Projects
{
    public interface IProjectEntity
    {
        Guid Id { get;  }
        Guid ProjectId { get; }
    }
}
