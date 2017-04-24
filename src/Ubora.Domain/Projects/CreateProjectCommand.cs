using System;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects
{
    public class CreateProjectCommand : ICommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public UserInfo UserInfo { get; set; }
    }
}