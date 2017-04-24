using System;
using Ubora.Domain.Events;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Projects
{
    public class CreateProjectCommand : ICommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public UserInfo UserInfo { get; set; }
    }
}