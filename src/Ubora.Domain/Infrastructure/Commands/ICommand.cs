using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Infrastructure.Commands
{
    public interface ICommand
    {
        UserInfo UserInfo { get; }
    }
}