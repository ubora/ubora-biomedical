using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Infrastructure.Commands
{
    public abstract class UserCommand : ICommand
    {
        public UserInfo UserInfo { get; set; }
    }
}