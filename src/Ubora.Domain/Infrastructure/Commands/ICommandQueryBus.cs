using Ubora.Domain.Infrastructure.Queries;

namespace Ubora.Domain.Infrastructure.Commands
{
    public interface ICommandQueryBus : ICommandBus, IQueryBus
    {
    }
}
