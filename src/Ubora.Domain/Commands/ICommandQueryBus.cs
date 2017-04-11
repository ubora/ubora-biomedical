using System;
using System.Collections.Generic;
using System.Text;

namespace Ubora.Domain.Commands
{
    public interface ICommandQueryBus : ICommandBus, IQueryBus
    {
    }
}
