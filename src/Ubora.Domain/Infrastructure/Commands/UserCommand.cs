using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Infrastructure.Commands
{
    public interface IUserCommand : ICommand
    {
        UserInfo Actor { get; set; }
    }

    public interface IProjectCommand : ICommand
    {
        Guid ProjectId { get; set; }
    }

    public abstract class UserProjectCommand : IUserCommand, IProjectCommand, ICommand
    {
        public UserInfo Actor { get; set; }
        public Guid ProjectId { get; set; }
    }

    public abstract class UserCommand : IUserCommand
    {
        public UserInfo Actor { get; set; }
    }
}