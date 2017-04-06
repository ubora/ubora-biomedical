using Ubora.Domain.Commands;

namespace Ubora.Domain.Projects
{
    public interface ICommandHandler<in T> where T : ICommand
    {
        void Handle(T command);
    }
}