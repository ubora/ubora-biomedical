using System;
using System.IO;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Projects.Repository
{
    public class AddFileCommand : UserProjectCommand
    {
        public Guid Id { get; set; }
        public Stream Stream { get; set; }
        public string FileName { get; set; }
    }
}
